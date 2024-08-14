using Vireo.Api.Core.Domain.Dtos.Requests;
using Vireo.Api.Core.Domain.Dtos.Responses;

namespace Vireo.Api.Core.Domain.Interfaces.Services;

public interface IClientService
{
    Task<PaginatedResult<GetClientResponse>> GetClientsAsync(int pageNumber, int pageSize);

    Task<GetClientResponse?> GetClientByIdAsync(Guid id);

    Task<Guid> AddClientAsync(CreateClientRequest client);

    Task UpdateClientAsync(UpdateClientRequest client, Guid id);

    Task DeleteClientAsync(Guid id);

    Task<PaginatedResult<GetClientResponse>> SearchClientsAsync(string? name, string? lastName, int pageNumber, int pageSize);
}
