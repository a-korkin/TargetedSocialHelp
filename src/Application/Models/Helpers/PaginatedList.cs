using Microsoft.EntityFrameworkCore;

namespace Application.Models.Helpers;

public class PaginatedList<TEntity>
{
    public List<TEntity> Items { get; } = new List<TEntity>();
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public PaginatedList(List<TEntity> items, int count, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = count;
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }

    public static async Task<PaginatedList<TEntity>> GetAsync(
        IQueryable<TEntity> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        int count = await source.CountAsync(cancellationToken);

        List<TEntity> items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<TEntity>(
            items: items,
            count: count,
            pageNumber: pageNumber,
            pageSize: pageSize);
    }
}