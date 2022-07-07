using Microsoft.EntityFrameworkCore;
using Domain.Entities.Admin;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<T> Set<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}