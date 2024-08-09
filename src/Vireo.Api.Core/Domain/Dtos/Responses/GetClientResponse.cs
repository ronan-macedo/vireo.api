namespace Vireo.Api.Core.Domain.Dtos.Responses;

public record GetClientResponse(
    Guid Id,
    string Name,
    string LastName,
    string Phone,
    string Email,
    bool Active,
    DateTimeOffset LastServiceDate);
