namespace Vireo.Api.Core.Domain.Dtos.Requests;

public record CreateClientRequest(
    string Name,
    string LastName,
    string Phone,
    string Email);
