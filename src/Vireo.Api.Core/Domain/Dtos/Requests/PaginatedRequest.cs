namespace Vireo.Api.Core.Domain.Dtos.Requests;

public record PaginatedRequest(int PageNumber = 1, int PageSize = 10);
