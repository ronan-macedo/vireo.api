using FluentValidation;
using Vireo.Api.Tests.Helpers.Dtos;

namespace Vireo.Api.Tests.Helpers.Validators;

public class DummyValidator : AbstractValidator<Dummy>
{
    public DummyValidator()
    {
        RuleFor(_ => _.Property1)
            .NotEmpty();
        RuleFor(_ => _.Property2)
            .NotEmpty();
    }
}
