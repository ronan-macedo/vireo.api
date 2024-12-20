﻿using Vireo.Api.Core.Domain.Dtos.Requests;
using Vireo.Api.Core.Domain.Dtos.Responses;
using Vireo.Api.Core.Domain.Entities;

namespace Vireo.Api.Core.Mappings;

public static class ClientMapper
{
    public static GetClientResponse ToGetClientResponse(this Client client)
    {
        return new GetClientResponse(
            client.Id,
            client.FirstName,
            client.LastName,
            client.Phone,
            client.Email,
            client.Active,
            client.LastServiceDate);
    }

    public static Client ToClient(this UpdateClientRequest request, Guid id)
    {
        return new Client
        {
            Id = id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            Email = request.Email
        };
    }

    public static Client ToClient(this CreateClientRequest request)
    {
        return new Client
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            Email = request.Email,
            Active = true,
            LastServiceDate = DateTimeOffset.UtcNow
        };
    }
}
