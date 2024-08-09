namespace Vireo.Api.Core.Notifications;

public class Notification
{
    public string Message { get; private set; }

    public Notification(string message) => Message = message;
}
