using Microsoft.EntityFrameworkCore;
using Vireo.Api.Infrastructure.Data.Context;

namespace Vireo.Api.Tests.Helpers.Data;

public class DatabaseFixture : IDisposable
{
    private bool disposedValue;

    public ApplicationDbContext DbContext { get; private set; }

    public DatabaseFixture()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        DbContext = new ApplicationDbContext(options);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                DbContext.Database.EnsureDeleted();
                DbContext.Dispose();
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
