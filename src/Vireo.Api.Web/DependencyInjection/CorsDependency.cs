using Vireo.Api.Core.Configurations;

namespace Vireo.Api.Web.DependencyInjection;

internal static class CorsDependency
{
    internal static void AddCorsDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var cors = new Cors();
        configuration.Bind(nameof(Cors), cors);
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(cors.ClientOrigin)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
    }
}
