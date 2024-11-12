using Vireo.Api.Core.Domain.Dtos.Requests;
using Vireo.Api.Core.Domain.Dtos.Responses;

namespace Vireo.Api.Core.Domain.Interfaces.Services;

public interface IClientService
{
    Task<PaginatedResult<GetClientResponse>> GetClientsAsync(PaginatedRequest paginatedRequest);

    Task<GetClientResponse?> GetClientByIdAsync(Guid id);

    Task<Guid> AddClientAsync(CreateClientRequest client);

    Task UpdateClientAsync(UpdateClientRequest client, Guid id);

    Task DeleteClientAsync(Guid id);

    Task<PaginatedResult<GetClientResponse>> SearchClientsAsync(SearchClientRequest searchClientRequest);
}
