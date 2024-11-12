using Microsoft.EntityFrameworkCore;
using Vireo.Api.Core.Domain.Interfaces.Repositories;
using Vireo.Api.Infrastructure.Data.Context;
using Vireo.Api.Infrastructure.Repositories;

namespace Vireo.Api.Web.DependencyInjection;

internal static class DataDependency
{
    internal static void AddDataDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string not found.");
        }
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<ApplicationDbContext>();
        services.AddScoped<IClientRepository, ClientRepository>();
    }
}
