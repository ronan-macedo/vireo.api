using Vireo.Api.Core.Domain.Dtos.Requests;
using Vireo.Api.Core.Domain.Dtos.Responses;
using Vireo.Api.Core.Domain.Entities;
using Vireo.Api.Core.Domain.Interfaces;
using Vireo.Api.Core.Domain.Interfaces.Repositories;
using Vireo.Api.Core.Domain.Interfaces.Services;
using Vireo.Api.Core.Mappings;
using Vireo.Api.Core.Notifications;

namespace Vireo.Api.Core.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    private readonly INotifier _notifier;

    public ClientService(IClientRepository clientRepository, INotifier notifier)
    {
        _clientRepository = clientRepository;
        _notifier = notifier;
    }

    public async Task<Guid> AddClientAsync(CreateClientRequest client)
    {
        var entity = client.ToClient();

        if (await _clientRepository.AddAsync(entity))
        {
            return entity.Id;
        }
        else
        {
            _notifier.AddNotification(new Notification("Um erro ocorreu ao adicionar um novo cliente."));
            return Guid.Empty;
        }
    }

    public async Task DeleteClientAsync(Guid id)
    {
        if (await _clientRepository.DeleteAsync(id))
        {
            return;
        }

        _notifier.AddNotification(new Notification("Um erro ocorreu ao deletar um cliente."));
    }

    public async Task<GetClientResponse?> GetClientByIdAsync(Guid id)
    {
        return await _clientRepository.GetByIdAsync(id) switch
        {
            null => null,
            var client => client.ToGetClientResponse()
        };
    }

    public async Task<PaginatedResult<GetClientResponse>> GetClientsAsync(int pageNumber, int pageSize)
    {
        PaginatedResult<Client> paginatedResult = await _clientRepository.GetClientsAsync(pageNumber, pageSize);

        return new PaginatedResult<GetClientResponse>(
            paginatedResult.Items.Select(client => client.ToGetClientResponse()),
            paginatedResult.TotalCount,
            pageSize,
            pageNumber);
    }

    public async Task UpdateClientAsync(UpdateClientRequest client, Guid id)
    {
        if (await _clientRepository.UpdateAsync(client.ToClient(id)))
        {
            return;
        }

        _notifier.AddNotification(new Notification("Um erro ocorreu ao modificar um cliente."));
    }

    public async Task<PaginatedResult<GetClientResponse>> SearchClientsAsync(
        string? name,
        string? lastName,
        int pageNumber,
        int pageSize)
    {
        PaginatedResult<Client> paginatedResult = await _clientRepository.SearchClientsAsync(
            name,
            lastName,
            pageNumber,
            pageSize);

        return new PaginatedResult<GetClientResponse>(
            paginatedResult.Items.Select(client => client.ToGetClientResponse()),
            paginatedResult.TotalCount,
            pageSize,
            pageNumber);
    }
}
