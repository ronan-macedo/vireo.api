using FluentValidation;

namespace Vireo.Api.Core.Domain.Interfaces.Services;

public interface IValidatorFactory
{
    IValidator<TObject> GetValidator<TObject>();
}
