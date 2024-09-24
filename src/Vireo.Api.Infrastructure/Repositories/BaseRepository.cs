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
        return await DbSet.AsNoTracking().FirstAsync(_ => _.Id == id);
    }

    public virtual async Task<bool> AddAsync(TEntity entity)
    {
        DbSet.Add(entity);
        return await Context.SaveChangesAsync() > 0;
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        TEntity? existingEntity = await DbSet.AsNoTracking().FirstOrDefaultAsync(c => c.Id == entity.Id);
        if (existingEntity == null)
        {
            return false;
        }
        existingEntity = entity;

        DbSet.Update(existingEntity);
        await Context.SaveChangesAsync();
        return true;
    }

    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        TEntity? existingEntity = await DbSet.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        if (existingEntity == null)
        {
            return false;
        }

        DbSet.Remove(existingEntity);
        await Context.SaveChangesAsync();
        return true;
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
