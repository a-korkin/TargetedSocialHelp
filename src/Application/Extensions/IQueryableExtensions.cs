using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Application.Models.Helpers;

namespace Application.Extensions;

public static class IQueryableExtensions
{
    public static Task<PaginatedList<T>> PaginatedListAsync<T>(
        this IQueryable<T> source,
        ResourceParameters request,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(request.Search)) source = source.Filter(request.Search);

        if (!string.IsNullOrWhiteSpace(request.Sort)) source = source.Order(request.Sort);

        return PaginatedList<T>.GetAsync(
            source,
            request.PageNumber,
            request.PageSize,
            cancellationToken);
    }

    private static IQueryable<T> Filter<T>(this IQueryable<T> source, string search)
    {
        Regex searchRegex = new(@"(\w+):(\w+)");
        string[] searchItems = search.Split(',');
        var parameter = Expression.Parameter(typeof(T));

        foreach (var item in searchItems)
        {
            string field = searchRegex.Match(item).Groups[1].Value;
            string term = searchRegex.Match(item).Groups[2].Value.ToLower();

            var property = Expression.Property(parameter, field);
            var propertyExp = Expression.Call(property, "ToLower", Type.EmptyTypes);
            var value = Expression.Constant(term, typeof(string));
            var valueExp = Expression.Call(value, "ToLower", Type.EmptyTypes);
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var containsMethodExp = Expression.Call(propertyExp, containsMethod!, valueExp);

            var lambda = Expression.Lambda<Func<T, bool>>(containsMethodExp, parameter);
            source = source.Where(lambda);
        }

        return source;
    }

    private static IOrderedQueryable<T> Order<T>(this IQueryable<T> source, string sort)
    {
        Regex sortingRegex = new(@"^(\w+)\((\w+)\)");
        string[] sortings = sort.Split(',');

        IOrderedQueryable<T>? result = null;

        foreach (var sorting in sortings.Select((value, index) => new { index, value }))
        {
            var order = sortingRegex.Match(sorting.value).Groups[1].Value.ToLower();
            var field = sortingRegex.Match(sorting.value).Groups[2].Value;

            if (sorting.index == 0)
            {
                result = OrderByField(source, order, field);

                if (sortings.Length == 1) return result;
            }
            result = OrderByField(result!, order + "_and_then", field);
        }

        return result!;
    }

    private static IOrderedQueryable<T> OrderByField<T>(
        IQueryable<T> source,
        string order,
        string field) => order switch
        {
            "asc" => source.OrderBy(field),
            "desc" => source.OrderByDescending(field),
            "asc_and_then" => ((IOrderedQueryable<T>)source).ThenBy(field),
            "desc_and_then" => ((IOrderedQueryable<T>)source).ThenByDescending(field),
            _ => throw new ArgumentException($"Order method not found for: ({order})")
        };

    private static IOrderedQueryable<T> OrderBy<T>(
        this IQueryable<T> source,
        string propertyName) => source.OrderBy(GetPropertyByName<T>(propertyName));

    private static IOrderedQueryable<T> OrderByDescending<T>(
        this IQueryable<T> source,
        string propertyName) => source.OrderByDescending(GetPropertyByName<T>(propertyName));

    private static IOrderedQueryable<T> ThenBy<T>(
        this IOrderedQueryable<T> source,
        string propertyName) => source.ThenBy(GetPropertyByName<T>(propertyName));

    private static IOrderedQueryable<T> ThenByDescending<T>(
        this IOrderedQueryable<T> source,
        string propertyName) => source.ThenByDescending(GetPropertyByName<T>(propertyName));

    private static Expression<Func<T, object>> GetPropertyByName<T>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(parameter, propertyName);
        var propAsObject = Expression.Convert(property, typeof(object));

        return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
    }
}