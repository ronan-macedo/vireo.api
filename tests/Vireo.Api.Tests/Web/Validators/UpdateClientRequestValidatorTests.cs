using FluentValidation.Results;
using Vireo.Api.Core.Domain.Dtos.Requests;
using Vireo.Api.Web.Validators;

namespace Vireo.Api.Tests.Web.Validators;

[Trait(TestTraits.UnitTest, TestTraits.ValidatorTest)]
public class UpdateClientRequestValidatorTests
{
    private readonly UpdateClientRequestValidator _sut;

    public UpdateClientRequestValidatorTests()
    {
        _sut = new UpdateClientRequestValidator();
    }

    [Theory]
    [InlineData("", "Doe", "1234567890", "john.doe@example.com")]
    [InlineData("John", "", "1234567890", "john.doe@example.com")]
    [InlineData("John", "Doe", "", "john.doe@example.com")]
    [InlineData("John", "Doe", "1234567890", "")]
    [InlineData("John", "Doe", "1234567890", "invalid-email")]
    public void UpdateClientRequestValidator_ShouldHaveError_WhenInvalidParameters(
        string firstName,
        string lastName,
        string phone,
        string email)
    {
        // Arrange
        var request = new UpdateClientRequest(firstName, lastName, phone, email);

        // Act
        ValidationResult result = _sut.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    [Fact]
    public void UpdateClientRequestValidator_ShouldNotHaveError_WhenRequestIsValid()
    {
        // Arrange
        var request = new UpdateClientRequest("John", "Doe", "1234567890", "john.doe@example.com");

        // Act
        ValidationResult result = _sut.Validate(request);

        // Assert
        Assert.True(result.IsValid);
    }
}
