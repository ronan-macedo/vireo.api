using Vireo.Api.Core.Domain.Dtos.Requests;
using Vireo.Api.Core.Domain.Dtos.Responses;
using Vireo.Api.Core.Domain.Entities;

namespace Vireo.Api.Core.Mappings;

public static class ClientMapper
{
    public static GetClientResponse ToGetClientResponse(this Client client)
    {
        return new GetClientResponse
        {
            Id = client.Id,
            Name = client.Name,
            LastName = client.LastName,
            Phone = client.Phone,
            Email = client.Email,
            Active = client.Active,
            LastServiceDate = client.LastServiceDate
        };
    }

    public static UpdateClientResponse ToUpdateClientResponse(this Client client)
    {
        return new UpdateClientResponse
        {
            Id = client.Id,
            Name = client.Name,
            LastName = client.LastName,
            Phone = client.Phone,
            Email = client.Email,
            Active = client.Active
        };
    }

    public static CreateClientResponse ToCreateClientResponse(this Client client)
    {
        return new CreateClientResponse
        {
            Id = client.Id,
            Name = client.Name,
            LastName = client.LastName
        };
    }

    public static Client ToClient(this UpdateClientRequest request, Guid id)
    {
        return new Client
        {
            Id = id,
            Name = request.Name,
            LastName = request.LastName,
            Phone = request.Phone,
            Email = request.Email
        };
    }

    public static Client ToClient(this CreateClientRequest request)
    {
        return new Client
        {
            Name = request.Name,
            LastName = request.LastName,
            Phone = request.Phone,
            Email = request.Email,
            Active = true,
            LastServiceDate = DateTimeOffset.UtcNow
        };
    }
}
