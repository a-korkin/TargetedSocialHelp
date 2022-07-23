using System.Linq.Expressions;
using Application.Models.Helpers;

namespace Application.Extensions;

public static class MappingExtension
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
}