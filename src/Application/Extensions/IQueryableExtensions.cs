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
        Expression<Func<T, bool>>? filter = null;
        // if (!string.IsNullOrWhiteSpace(request.Search))
        // {
        //     if (request.Search.Contains(':'))
        //     {
        //         foreach (var searchTerm in request.Search.Split(','))
        //         {
        //             Regex searchReg = new(@"(\w+):(\w+)");
        //             string field = searchReg.Match(searchTerm).Groups[1].Value;
        //             string term = searchReg.Match(request.Search).Groups[2].Value;
        //             Console.WriteLine($"field: {field}, term: {term}");
        //         }
        //     }
        // }

        Func<IQueryable<T>, IOrderedQueryable<T>>? orderedQuery = null;
        if (!string.IsNullOrWhiteSpace(request.Sort)) orderedQuery = u => u.Order(request.Sort);

        return PaginatedList<T>.GetAsync(
            source,
            request.PageNumber,
            request.PageSize,
            filter,
            orderedQuery,
            cancellationToken);
    }

    private static IOrderedQueryable<T> Order<T>(
        this IQueryable<T> source,
        string sort)
    {
        Regex orderReg = new(@"^(\w+)\(");
        Regex fieldReg = new(@"\((\w+)\)");
        string[] sortings = sort.Split(',');

        IOrderedQueryable<T>? result = null;

        foreach (var sorting in sortings.Select((value, index) => new { index, value }))
        {
            var field = fieldReg.Match(sorting.value).Groups[1].Value;
            var order = orderReg.Match(sorting.value).Groups[1].Value.ToLower();

            if (sorting.index == 0)
            {
                result = OrderByField<T>(source, order, field);

                if (sortings.Length == 1) return result;
            }
            result = OrderByField<T>(result!, order + "_and_then", field);
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