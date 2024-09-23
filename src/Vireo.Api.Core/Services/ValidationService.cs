using FluentValidation;
using FluentValidation.Results;
using Vireo.Api.Core.Domain.Dtos.Responses;
using Vireo.Api.Core.Domain.Interfaces.Services;
using IValidatorFactory = Vireo.Api.Core.Domain.Interfaces.Services.IValidatorFactory;

namespace Vireo.Api.Core.Services;

public class ValidationService : IValidationService
{
    public bool IsValid { get; private set; }

    public ErrorResponse? GetErrorResponse { get; private set; }

    private readonly IValidatorFactory _validatorFactory;

    public ValidationService(IValidatorFactory validatorFactory)
    {
        _validatorFactory = validatorFactory;
    }

    public void Validate<TRequest>(TRequest request) where TRequest : class
    {
        IValidator<TRequest> validator = _validatorFactory.GetValidator<TRequest>();
        ValidationResult validationResult = validator.Validate(request);
        IsValid = validationResult.IsValid;

        if (!validationResult.IsValid)
        {
            GetErrorResponse = new ErrorResponse("Validation failed", validationResult.Errors.Select(x => x.ErrorMessage));
        }
    }
}
