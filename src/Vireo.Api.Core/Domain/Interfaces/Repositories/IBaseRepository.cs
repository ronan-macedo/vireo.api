using Vireo.Api.Core.Domain.Entities;

namespace Vireo.Api.Core.Domain.Interfaces.Repositories;

public interface IBaseRepository<TEntity> : IDisposable where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(Guid id);

    Task<bool> AddAsync(TEntity entity);

    Task<bool> UpdateAsync(TEntity entity);

    Task<bool> DeleteAsync(Guid id);
}
