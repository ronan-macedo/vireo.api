using FluentValidation;
using Vireo.Api.Core.Domain.Dtos.Requests;

namespace Vireo.Api.Web.Validators;

public class CreateClientRequestValidator : AbstractValidator<CreateClientRequest>
{
    public CreateClientRequestValidator()
    {
        RuleFor(_ => _.FirstName)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .MaximumLength(100)
            .WithMessage("{PropertyName} cannot be longer than {MaxLength}.");

        RuleFor(_ => _.LastName)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .MaximumLength(100)
            .WithMessage("{PropertyName} cannot be longer than {MaxLength}.");

        RuleFor(_ => _.Phone)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .Matches(@"^\d{9,13}$")
            .WithMessage("{PropertyName} must be between 9 and 13 digits.");

        RuleFor(_ => _.Email)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .MaximumLength(50)
            .WithMessage("{PropertyName} cannot be longer than {MaxLength}.")
            .EmailAddress()
            .WithMessage("{PropertyName} is not valid.");
    }
}
