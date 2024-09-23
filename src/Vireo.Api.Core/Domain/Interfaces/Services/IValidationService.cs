using Vireo.Api.Core.Domain.Dtos.Responses;

namespace Vireo.Api.Core.Domain.Interfaces.Services;

public interface IValidationService
{
    bool IsValid { get; }

    ErrorResponse? GetErrorResponse { get; }

    void Validate<TRequest>(TRequest request) where TRequest : class;
}
