namespace Vireo.Api.Core.Domain.Dtos.Requests;

public record SearchClientRequest(
    string? FirstName,
    string? LastName,
    int PageNumber = 1,
    int PageSize = 10)
    : PaginatedRequest(PageNumber, PageSize);
