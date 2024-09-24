namespace Vireo.Api.Core.Domain.Dtos.Responses;

public record GetClientResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Phone,
    string Email,
    bool Active,
    DateTimeOffset LastServiceDate);
