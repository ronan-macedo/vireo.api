﻿using Microsoft.EntityFrameworkCore;
using Vireo.Api.Core.Domain.Entities;
using Vireo.Api.Core.Domain.Interfaces.Repositories;
using Vireo.Api.Infrastructure.Data.Context;

namespace Vireo.Api.Infrastructure.Data.Repositories;

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

    public virtual async Task AddAsync(TEntity entity)
    {
        DbSet.Add(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        DbSet.Update(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        DbSet.Remove(new TEntity { Id = id });
        await Context.SaveChangesAsync();
    }

    public void Dispose()
    {
        if (!_disposedValue)
        {
            Context.Dispose();
            _disposedValue = true;
        }
        GC.SuppressFinalize(this);
    }
}
