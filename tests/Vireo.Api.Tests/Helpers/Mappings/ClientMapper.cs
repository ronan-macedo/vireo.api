using Vireo.Api.Core.Domain.Dtos.Requests;
using Vireo.Api.Core.Domain.Dtos.Responses;
using Vireo.Api.Core.Domain.Entities;

namespace Vireo.Api.Tests.Helpers.Mappings;

internal static class ClientMapper
{
    internal static GetClientResponse ToGetClientResponse(this CreateClientRequest client, Guid id)
    {
        return new GetClientResponse(
            id,
            client.FirstName,
            client.LastName,
            client.Phone,
            client.Email,
            true,
            DateTimeOffset.UtcNow);
    }
}
