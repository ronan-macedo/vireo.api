namespace Vireo.Api.Core.Domain.Dtos.Requests;

public record UpdateClientRequest(
    string FirstName,
    string LastName,
    string Phone,
    string Email);
