namespace Vireo.Api.Core.Domain.Dtos.Requests;

public class UpdateClientRequest
{
    public string Name { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
}
