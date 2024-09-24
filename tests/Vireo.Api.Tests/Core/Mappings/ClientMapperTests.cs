using AutoFixture;
using Vireo.Api.Core.Domain.Dtos.Requests;
using Vireo.Api.Core.Domain.Entities;
using Vireo.Api.Core.Mappings;

namespace Vireo.Api.Tests.Core.Mappings;

[Trait(TestTraits.UnitTest, TestTraits.MappingTest)]
public class ClientMapperTests
{
    private readonly IFixture _fixture = new Fixture();

    [Fact]
    public void ToGetClientResponse_FromClient_ShouldMapCorrectly()
    {
        // Arrange
        Client client = _fixture.Create<Client>();

        // Act
        var response = client.ToGetClientResponse();

        // Assert
        Assert.Equal(client.Id, response.Id);
        Assert.Equal(client.FirstName, response.FirstName);
        Assert.Equal(client.LastName, response.LastName);
        Assert.Equal(client.Phone, response.Phone);
        Assert.Equal(client.Email, response.Email);
        Assert.Equal(client.Active, response.Active);
        Assert.Equal(client.LastServiceDate, response.LastServiceDate);
    }

    [Fact]
    public void ToClient_FromUpdateClientRequest_ShouldMapCorrectly()
    {
        // Arrange
        var request = _fixture.Create<UpdateClientRequest>();
        var id = Guid.NewGuid();

        // Act
        var client = request.ToClient(id);

        // Assert
        Assert.Equal(id, client.Id);
        Assert.Equal(request.FirstName, client.FirstName);
        Assert.Equal(request.LastName, client.LastName);
        Assert.Equal(request.Phone, client.Phone);
        Assert.Equal(request.Email, client.Email);
    }

    [Fact]
    public void ToClient_FromCreateClientRequest_ShouldMapCorrectly()
    {
        // Arrange
        var request = _fixture.Create<CreateClientRequest>();

        // Act
        var client = request.ToClient();

        // Assert
        Assert.Equal(request.FirstName, client.FirstName);
        Assert.Equal(request.LastName, client.LastName);
        Assert.Equal(request.Phone, client.Phone);
        Assert.Equal(request.Email, client.Email);
        Assert.True(client.Active);
        Assert.True((DateTimeOffset.UtcNow - client.LastServiceDate).TotalSeconds < 1);
    }
}
