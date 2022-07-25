using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Application.Models.Helpers;

namespace Application.Extensions;

public static class IQueryableExtensions
{
    public static Task<PaginatedList<T>> PaginatedListAsync<T>(
        this IQueryable<T> source,
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderedQuery = null)
    {
        return PaginatedList<T>.GetAsync(source, pageNumber, pageSize, filter, orderedQuery);
    }

    public static IOrderedQueryable<T> OrderBy<T>(
        this IQueryable<T> source,
        string propertyName)
    {
        return source.OrderBy(ToLambda<T>(propertyName));
    }

    public static IOrderedQueryable<T> OrderByDescending<T>(
        this IQueryable<T> source,
        string propertyName)
    {
        return source.OrderByDescending(ToLambda<T>(propertyName));
    }

    public static IOrderedQueryable<T> ThenBy<T>(
        this IOrderedQueryable<T> source,
        string propertyName)
    {
        return source.ThenBy(ToLambda<T>(propertyName));
    }

    public static IOrderedQueryable<TEntity> ThenByDescending<TEntity>(
        this IOrderedQueryable<TEntity> source,
        string propertyName)
    {
        return source.ThenByDescending(ToLambda<TEntity>(propertyName));
    }

    private static Expression<Func<TEntity, object>> ToLambda<TEntity>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(TEntity));
        var property = Expression.Property(parameter, propertyName);
        var propAsObject = Expression.Convert(property, typeof(object));

        return Expression.Lambda<Func<TEntity, object>>(propAsObject, parameter);
    }

    public static IOrderedQueryable<T> Sorted<T>(
        this IQueryable<T> source,
        string sort)
    {
        Regex orderReg = new(@"^(\w{3,4})\(");
        Regex fieldReg = new(@"\((\w+)\)");

        string[] sortings = sort.Split(',');
        IOrderedQueryable<T> result = source.OrderBy(_ => 0);
        foreach (string sorting in sortings)
        {
            var field = fieldReg.Match(sorting).Groups[1].Value;
            var order = orderReg.Match(sorting).Groups[1].Value;

            result = Sort<T>(result, order, field);
        }

        return result;
    }

    private static IOrderedQueryable<T> Sort<T>(
        IOrderedQueryable<T> source,
        string order,
        string field) => order switch
        {
            "asc" => source.ThenBy(field),
            "desc" => source.ThenByDescending(field),
            _ => throw new ArgumentException($"Order method not found for: ({order})")
        };
}