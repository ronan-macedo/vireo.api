using Vireo.Api.Core.Domain.Entities;

namespace Vireo.Api.Core.Domain.Interfaces.Repositories;

public interface IBaseRepository<TEntity> : IDisposable where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(Guid id);

    Task AddAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(Guid id);
}
