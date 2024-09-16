using Vireo.Api.Core.Domain.Interfaces;

namespace Vireo.Api.Core.Notifications;

public sealed class Notifier : INotifier
{
    private List<Notification> Notifications { get; set; }

    public Notifier()
    {
        Notifications = [];
    }

    public bool HasNotification => Notifications.Count != 0;

    public ICollection<Notification> GetNotifications => Notifications;

    public void AddNotification(Notification notification) => Notifications.Add(notification);
}
