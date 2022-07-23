using System.Linq.Expressions;
using Application.Models.Helpers;

namespace Application.Extensions;

public static class IQueryableExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> source,
        int pageNumber,
        int pageSize,
        Expression<Func<TDestination, bool>>? filter = null,
        Func<IQueryable<TDestination>, IOrderedQueryable<TDestination>>? orderedQuery = null)
    {
        return PaginatedList<TDestination>.GetAsync(source, pageNumber, pageSize, filter, orderedQuery);
    }

    public static IOrderedQueryable<TEntity> OrderBy<TEntity>(
        this IQueryable<TEntity> source,
        string propertyName)
    {
        return source.OrderBy(ToLambda<TEntity>(propertyName));
    }

    public static IOrderedQueryable<TEntity> OrderByDescending<TEntity>(
        this IQueryable<TEntity> source,
        string propertyName)
    {
        return source.OrderByDescending(ToLambda<TEntity>(propertyName));
    }

    private static Expression<Func<TEntity, object>> ToLambda<TEntity>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(TEntity));
        var property = Expression.Property(parameter, propertyName);
        var propAsObject = Expression.Convert(property, typeof(object));

        return Expression.Lambda<Func<TEntity, object>>(propAsObject, parameter);
    }
}