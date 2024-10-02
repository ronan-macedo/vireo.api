using Microsoft.Extensions.DependencyInjection;
using NuGet.Protocol.Plugins;
using Vireo.Api.Infrastructure.Data.Context;
using Vireo.Api.Tests.Helpers.Integration;

namespace Vireo.Api.Tests.Integration;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory<Program>>, IDisposable
{
    private bool disposedValue;

    private readonly IServiceScope _scope;

    protected readonly ApplicationDbContext DbContext;

    protected readonly HttpClient Client;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory<Program> factory)
    {
        _scope = factory.Services.CreateScope();

        DbContext = _scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        Client = factory.CreateClient();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Client.Dispose();
                DbContext.Dispose();
                _scope.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
