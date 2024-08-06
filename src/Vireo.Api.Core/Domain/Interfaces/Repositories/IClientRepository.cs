using Vireo.Api.Core.Domain.Entities;

namespace Vireo.Api.Core.Domain.Interfaces.Repositories;

public interface IClientRepository : IBaseRepository<Client>
{
    Task<IEnumerable<Client>> GetClientsAsync();
}
