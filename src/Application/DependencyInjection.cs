using System.Reflection;
using Application.Profiles;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MapperProfile));
        services.AddMediatR(Assembly.GetExecutingAssembly());
        return services;
    }
}