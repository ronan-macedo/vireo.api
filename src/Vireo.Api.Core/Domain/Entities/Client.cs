namespace Vireo.Api.Core.Domain.Entities;

public class Client : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool Active { get; set; }

    public DateTimeOffset LastServiceDate { get; set; }
}
