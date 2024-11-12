using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using IValidatorFactory = Vireo.Api.Core.Domain.Interfaces.Services.IValidatorFactory;

namespace Vireo.Api.Core.Services;

public class ValidatorFactory : IValidatorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ValidatorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IValidator<TObject> GetValidator<TObject>()
    {
        IValidator<TObject>? validator = _serviceProvider.GetService<IValidator<TObject>>();

        return validator ?? throw new InvalidOperationException($"No validator found for type {typeof(TObject).Name}");
    }
}
