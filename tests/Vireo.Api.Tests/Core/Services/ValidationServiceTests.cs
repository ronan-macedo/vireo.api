using AutoFixture;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using Vireo.Api.Core.Services;
using Vireo.Api.Tests.Helpers.Dtos;
using IValidatorFactory = Vireo.Api.Core.Domain.Interfaces.Services.IValidatorFactory;

namespace Vireo.Api.Tests.Core.Services;

[Trait(TestTraits.UnitTest, TestTraits.ServiceTest)]
public class ValidationServiceTests
{
    private readonly IValidatorFactory _validatorFactory = Substitute.For<IValidatorFactory>();

    private readonly IValidator<Dummy> _validator = Substitute.For<IValidator<Dummy>>();

    public readonly IFixture _fixture = new Fixture();

    public readonly ValidationService _sut;

    public ValidationServiceTests()
    {
        _sut = new ValidationService(_validatorFactory);
    }

    [Fact]
    public void Validate_WhenValidationFails_ShouldIsValidFalse()
    {
        // Arrange
        Dummy dummy = _fixture.Create<Dummy>();
        _validatorFactory.GetValidator<Dummy>().Returns(_validator);
        ValidationResult validationResult = _fixture.Create<ValidationResult>();
        _validator.Validate(Arg.Any<Dummy>()).Returns(validationResult);

        // Act
        _sut.Validate(dummy);

        // Assert
        Assert.False(_sut.IsValid);
        Assert.NotNull(_sut.GetErrorResponse);
    }

    [Fact]
    public void Validate_WhenValidationPasses_ShouldIsValidTrue()
    {
        // Arrange
        Dummy dummy = _fixture.Create<Dummy>();
        _validatorFactory.GetValidator<Dummy>().Returns(_validator);
        var validationResult = new ValidationResult();
        _validator.Validate(Arg.Any<Dummy>()).Returns(validationResult);

        // Act
        _sut.Validate(dummy);

        // Assert
        Assert.True(_sut.IsValid);
        Assert.Null(_sut.GetErrorResponse);
    }
}
