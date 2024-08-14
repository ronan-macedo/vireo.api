using System.ComponentModel.DataAnnotations;
using Vireo.Api.Core.Domain.Dtos.Responses;

namespace Vireo.Api.Web.Attribute;

public class GuidNotEmptyAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is Guid guid && guid == Guid.Empty)
        {
            var errorMessage = new ErrorResponse(
                "Validation failed",
                ["'Id' não pode ser nulo ou vázio"]
            );

            return new ValidationResult(errorMessage.Message);
        }

        return ValidationResult.Success;
    }
}
