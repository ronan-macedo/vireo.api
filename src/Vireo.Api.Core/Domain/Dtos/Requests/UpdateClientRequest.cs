namespace Vireo.Api.Core.Domain.Dtos.Requests;

public record UpdateClientRequest(
    string Name,
    string LastName,
    string Phone,
    string Email);
