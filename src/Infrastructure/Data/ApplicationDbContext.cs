using Application.Interfaces;
using Domain.Entities.Admin;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IAuthService _authService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IAuthService authService) : base(options)
    {
        _authService = authService;
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Seed(_authService);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}