using FluentValidation.Results;
using Vireo.Api.Core.Domain.Dtos.Requests;
using Vireo.Api.Web.Validators;

namespace Vireo.Api.Tests.Web.Validators;

[Trait(TestTraits.UnitTest, TestTraits.ValidatorTest)]
public class CreateClientRequestValidatorTests
{
    private readonly CreateClientRequestValidator _validator;

    public CreateClientRequestValidatorTests()
    {
        _validator = new CreateClientRequestValidator();
    }

    [Theory]
    [InlineData("", "Doe", "1234567890", "john.doe@example.com")]
    [InlineData("John", "", "1234567890", "john.doe@example.com")]
    [InlineData("John", "Doe", "", "john.doe@example.com")]
    [InlineData("John", "Doe", "1234567890", "")]
    [InlineData("John", "Doe", "1234567890", "invalid-email")]
    public void Validator_ShouldHaveError_WhenInvalidParameters(
        string firstName,
        string lastName,
        string phone,
        string email)
    {
        // Arrange
        var request = new CreateClientRequest(firstName, lastName, phone, email);

        // Act
        ValidationResult result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    [Fact]
    public void ShouldNotHaveErrorWhenRequestIsValid()
    {
        // Arrange
        var request = new CreateClientRequest("John", "Doe", "1234567890", "john.doe@example.com");

        // Act
        ValidationResult result = _validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
    }
}
