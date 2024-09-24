using Microsoft.EntityFrameworkCore;
using Vireo.Api.Infrastructure.Data.Context;

namespace Vireo.Api.Tests.Helpers.Data;

public class DatabaseFixture : IDisposable
{
    private bool disposedValue;

    public ApplicationContext Context { get; private set; }

    public DatabaseFixture()
    {
        DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        Context = new ApplicationContext(options);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Context.Database.EnsureDeleted();
                Context.Dispose();
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
