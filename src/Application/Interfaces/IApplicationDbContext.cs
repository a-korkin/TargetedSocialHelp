using System;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Admin;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
}