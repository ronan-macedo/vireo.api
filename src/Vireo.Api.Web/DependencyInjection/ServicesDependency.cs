using FluentValidation;
using Vireo.Api.Core.Domain.Dtos.Requests;
using Vireo.Api.Core.Domain.Interfaces;
using Vireo.Api.Core.Domain.Interfaces.Services;
using Vireo.Api.Core.Notifications;
using Vireo.Api.Core.Services;
using Vireo.Api.Web.Validators;
using IValidatorFactory = Vireo.Api.Core.Domain.Interfaces.Services.IValidatorFactory;

namespace Vireo.Api.Web.DependencyInjection;

internal static class ServicesDependency
{
    internal static void AddServicesDependencies(this IServiceCollection services)
    {
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<INotifier, Notifier>();

        services.AddTransient<IValidator<CreateClientRequest>, CreateClientRequestValidator>();
        services.AddTransient<IValidator<UpdateClientRequest>, UpdateClientRequestValidator>();
        services.AddSingleton<IValidatorFactory, ValidatorFactory>();
        services.AddTransient<IValidationService, ValidationService>();
    }
}
