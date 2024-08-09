using Microsoft.EntityFrameworkCore;
using Vireo.Api.Core.Domain.Interfaces.Repositories;
using Vireo.Api.Infrastructure.Data.Context;
using Vireo.Api.Infrastructure.Repositories;

namespace Vireo.Api.Web.DependencyInjection.Extensions;

internal static class DataDependenciesHandler
{
    internal static void AddDataDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<ApplicationContext>();
        services.AddScoped<IClientRepository, ClientRepository>();
    }
}
