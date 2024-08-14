namespace Vireo.Api.Core.Domain.Dtos.Responses;

public record ErrorResponse(string Message, IEnumerable<string> Errors);
