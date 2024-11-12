using Vireo.Api.Core.Notifications;

namespace Vireo.Api.Core.Domain.Interfaces;

public interface INotifier
{
    bool HasNotification { get; }

    ICollection<Notification> GetNotifications { get; }

    void AddNotification(Notification notification);
}
