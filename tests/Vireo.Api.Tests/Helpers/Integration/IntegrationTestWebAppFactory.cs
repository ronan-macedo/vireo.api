using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Vireo.Api.Infrastructure.Data.Context;

namespace Vireo.Api.Tests.Helpers.Integration;

public class IntegrationTestWebAppFactory<TProgram>
    : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("vireo")
        .WithPassword("vireo_password")
        .Build();

    public Task InitializeAsync()
    {
        return _postgreSqlContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing ApplicationContext registration.
            ServiceDescriptor? currentDbContext = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (currentDbContext != null)
            {
                services.Remove(currentDbContext);
            }

            // Add ApplicationContext using an in-memory database for testing.
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(_postgreSqlContainer.GetConnectionString());
            });

            services.AddSingleton<IPolicyEvaluator, DummyPolicyEvaluator>();

            // Ensure the database is created.
            ServiceProvider sp = services.BuildServiceProvider();
            using (IServiceScope scope = sp.CreateScope())
            {
                IServiceProvider scopedServices = scope.ServiceProvider;
                ApplicationDbContext db = scopedServices.GetRequiredService<ApplicationDbContext>();

                db.Database.EnsureCreated();
            }
        });

        builder.UseEnvironment("Development");
    }

    public new Task DisposeAsync()
    {
        return _postgreSqlContainer.StopAsync();
    }
}
