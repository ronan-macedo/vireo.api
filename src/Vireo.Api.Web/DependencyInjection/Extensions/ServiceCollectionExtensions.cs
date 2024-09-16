using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using Vireo.Api.Infrastructure.Data.Context;

namespace Vireo.Api.Web.DependencyInjection.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        services.AddDataDependencies(configuration);
        services.AddServicesDependencies();
        services.AddControllers();
        services.ConfigureHttpJsonOptions(options => options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

        // Add health checks
        string? connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string not found.");
        }
        services.AddHealthChecks()
            .AddNpgSql(connectionString);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }

            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Vireo API", Version = "v1" });
        });
    }
}
