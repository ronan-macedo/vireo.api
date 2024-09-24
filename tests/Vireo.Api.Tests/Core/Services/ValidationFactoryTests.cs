using FluentValidation;
using NSubstitute;
using Vireo.Api.Core.Services;
using Vireo.Api.Tests.Helpers.Dtos;

namespace Vireo.Api.Tests.Core.Services;

[Trait(TestTraits.UnitTest, TestTraits.ServiceTest)]
public class ValidationFactoryTests
{
    [Fact]
    public void GetValidator_ShouldReturnValidator()
    {
        // Arrange
        IServiceProvider serviceProvider = Substitute.For<IServiceProvider>();
        IValidator<Dummy> validator = Substitute.For<IValidator<Dummy>>();
        serviceProvider.GetService(typeof(IValidator<Dummy>)).Returns(validator);

        var factory = new ValidatorFactory(serviceProvider);

        // Act
        IValidator<Dummy> result = factory.GetValidator<Dummy>();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ValidatorFactory>(factory);
    }

    [Fact]
    public void GetValidator_ShouldReturnNull_WhenValidatorNotRegistered()
    {
        // Arrange
        IServiceProvider serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider.GetService(typeof(IValidator<Dummy>)).Returns(null);

        var factory = new ValidatorFactory(serviceProvider);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
        {
            factory.GetValidator<Dummy>();
        });
    }
}
