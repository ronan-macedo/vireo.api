using Vireo.Api.Core.Domain.Interfaces;
using Vireo.Api.Core.Domain.Interfaces.Services;
using Vireo.Api.Core.Notifications;
using Vireo.Api.Core.Services;

namespace Vireo.Api.Web.DependencyInjection.Extensions;

internal static class ServicesDependenciesHandler
{
    public static void AddServicesDependencies(this IServiceCollection services)
    {
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<INotifier, Notifier>();
    }
}
