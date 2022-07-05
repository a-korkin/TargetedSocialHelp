using Application.Interfaces;
using Domain.Entities.Admin;
using Microsoft.EntityFrameworkCore;

namespace Application.Test.Helpers;

public class MockApplicationDbContext : IApplicationDbContext
{
    public DbSet<User> Users => throw new NotImplementedException();

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}