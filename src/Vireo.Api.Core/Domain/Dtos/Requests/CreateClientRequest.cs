namespace Vireo.Api.Core.Domain.Dtos.Requests;

public record CreateClientRequest(
    string FirstName,
    string LastName,
    string Phone,
    string Email);
