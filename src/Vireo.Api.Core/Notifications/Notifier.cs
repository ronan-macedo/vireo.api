using Vireo.Api.Core.Domain.Interfaces;

namespace Vireo.Api.Core.Notifications;

public class Notifier : INotifier
{
    private List<Notification> _notifications;

    public Notifier()
    {
        _notifications = [];
    }

    public bool HasNotification => _notifications.Count != 0;

    public ICollection<Notification> Notifications => _notifications;

    public void AddNotification(Notification notification) => _notifications.Add(notification);
}
