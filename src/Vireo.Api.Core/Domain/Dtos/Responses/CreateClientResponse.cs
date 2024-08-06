namespace Vireo.Api.Core.Domain.Dtos.Responses;

public class CreateClientResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
}
