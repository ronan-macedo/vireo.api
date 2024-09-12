using Microsoft.EntityFrameworkCore;
using Vireo.Api.Core.Domain.Entities;
using Vireo.Api.Core.Domain.Interfaces.Repositories;
using Vireo.Api.Infrastructure.Data.Context;

namespace Vireo.Api.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity, new()
{
    protected readonly ApplicationContext Context;

    protected readonly DbSet<TEntity> DbSet;

    private bool _disposedValue;

    protected BaseRepository(ApplicationContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task<bool> AddAsync(TEntity entity)
    {
        DbSet.Add(entity);
        return await Context.SaveChangesAsync() > 0;
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        DbSet.Update(entity);
        return await Context.SaveChangesAsync() > 0;
    }

    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        DbSet.Remove(new TEntity { Id = id });
        return await Context.SaveChangesAsync() > 0;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            Context.Dispose();
            _disposedValue = disposing;
        }
    }
}
