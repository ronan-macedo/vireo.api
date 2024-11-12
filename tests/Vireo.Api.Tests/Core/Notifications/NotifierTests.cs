using Vireo.Api.Core.Notifications;

namespace Vireo.Api.Tests.Core.Notifications;

[Trait(TestTraits.UnitTest, TestTraits.NotifierTest)]
public class NotifierTests
{
    private readonly Notifier _sut;

    public NotifierTests()
    {
        _sut = new Notifier();
    }

    [Fact]
    public void Notifier_ShouldHaveNotification_WhenAddNotification()
    {
        // Arrange
        var notification = new Notification("Test");

        // Act
        _sut.AddNotification(notification);

        // Assert
        Assert.True(_sut.HasNotification);
    }

    [Fact]
    public void Notifier_ShouldNotHaveNotification_WhenNoNotification()
    {
        // Assert
        Assert.False(_sut.HasNotification);
    }

    [Fact]
    public void Notifier_ShouldReturnNotification_WhenGetNotification()
    {
        // Arrange
        var notification = new Notification("Test");

        // Act
        _sut.AddNotification(notification);

        // Assert
        Assert.Contains(notification, _sut.GetNotifications);
    }
}
