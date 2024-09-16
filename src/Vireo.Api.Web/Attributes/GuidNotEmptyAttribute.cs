using System.ComponentModel.DataAnnotations;
using Vireo.Api.Core.Domain.Dtos.Responses;

namespace Vireo.Api.Web.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class GuidNotEmptyAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is Guid guid && guid == Guid.Empty)
        {
            var errorMessage = new ErrorResponse(
                "Validation failed",
                ["The value should not be null or 00000000-0000-0000-0000-000000000000"]
            );

            return new ValidationResult(errorMessage.Message);
        }

        return ValidationResult.Success;
    }
}
