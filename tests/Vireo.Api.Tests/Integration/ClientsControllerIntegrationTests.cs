using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Vireo.Api.Core.Domain.Dtos.Requests;
using Vireo.Api.Core.Domain.Dtos.Responses;
using Vireo.Api.Core.Domain.Entities;
using Vireo.Api.Infrastructure.Data.Context;
using Vireo.Api.Tests.Helpers.Integration;

namespace Vireo.Api.Tests.Integration;

[Trait(TestTraits.UnitTest, TestTraits.IntegrationTest)]
public class ClientsControllerIntegrationTests : BaseIntegrationTest
{
    public ClientsControllerIntegrationTests(IntegrationTestWebAppFactory<Program> factory) : base(factory)
    { }

    #region CreateClientAsync

    [Fact]
    public async Task CreateClientAsync_WhenClientIsValid_ShouldCreateClient()
    {
        // Arrange
        CreateClientRequest clientToCreate = new("User", "Test", "111222333", "user@email.com");

        // Act
        HttpResponseMessage response = await Client.PostAsJsonAsync("/v1/clients", clientToCreate);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        GetClientResponse? result = await response.Content.ReadFromJsonAsync<GetClientResponse>();
        Assert.NotNull(result);
        Assert.Equal(clientToCreate.FirstName, result.FirstName);
        Assert.Equal(clientToCreate.LastName, result.LastName);
        Assert.Equal(clientToCreate.Phone, result.Phone);
        Assert.Equal(clientToCreate.Email, result.Email);
    }

    [Fact]
    public async Task CreateClientAsync_WhenClientIsInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        CreateClientRequest clientToCreate = new("User", "Test", "111222333", "useremail.com");

        // Act
        HttpResponseMessage response = await Client.PostAsJsonAsync("/v1/clients", clientToCreate);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion CreateClientAsync

    #region GetClientAsync

    [Fact]
    public async Task GetClientsAsync_WhenClientsExist_ShouldReturnClients()
    {
        // Arrange & Act
        HttpResponseMessage response = await Client.GetAsync("/v1/clients");

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    #endregion GetClientAsync

    #region GetClientByIdAsync

    [Fact]
    public async Task GetClientByIdAsync_WhenClientExists_ShouldReturnClient()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = new Client
        {
            Id = clientId,
            FirstName = "Test",
            LastName = "Client",
            Email = "test@example.com",
            Phone = "1234567890",
            Active = true,
            LastServiceDate = DateTimeOffset.UtcNow
        };
        DbContext.Clients.Add(client);
        await DbContext.SaveChangesAsync();

        // Act
        HttpResponseMessage response = await Client.GetAsync($"/v1/clients/{clientId}");

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        GetClientResponse? result = await response.Content.ReadFromJsonAsync<GetClientResponse>();
        Assert.NotNull(result);
        Assert.Equal(client.FirstName, result.FirstName);
        Assert.Equal(client.LastName, result.LastName);
        Assert.Equal(client.Phone, result.Phone);
        Assert.Equal(client.Email, result.Email);
    }

    [Fact]
    public async Task GetClientByIdAsync_WhenClientDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await Client.GetAsync($"/v1/clients/{id}");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion GetClientByIdAsync

    #region UpdateClientAsync

    [Fact]
    public async Task UpdateClientAsync_WhenClientExists_ShouldUpdateClient()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = new Client
        {
            Id = clientId,
            FirstName = "Test",
            LastName = "Client",
            Email = "test@example.com",
            Phone = "1234567890",
            Active = true,
            LastServiceDate = DateTimeOffset.UtcNow
        };
        DbContext.Clients.Add(client);
        await DbContext.SaveChangesAsync();
        DbContext.Entry(client).State = EntityState.Detached;
        var updatedClient = new UpdateClientRequest(
            "Updated",
            "Client",
            "123456789",
            "test@example.com");

        // Act
        HttpResponseMessage response = await Client.PutAsJsonAsync($"/v1/clients/{clientId}", updatedClient);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateClientAsync_WhenClientDoesNotExist_ShouldReturnBadRequest()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updatedClient = new UpdateClientRequest(
            "Updated",
            "Client",
            "123456789",
            "test@example.com");

        // Act
        HttpResponseMessage response = await Client.PutAsJsonAsync($"/v1/clients/{id}", updatedClient);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateClientAsync_WhenClientIsInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updatedClient = new UpdateClientRequest(
            "Updated",
            "Client",
            "testexample.com",
            string.Empty);

        // Act
        HttpResponseMessage response = await Client.PutAsJsonAsync($"/v1/clients/{id}", updatedClient);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion UpdateClientAsync

    #region DeleteClientAsync

    [Fact]
    public async Task DeleteClientAsync_WhenClientExists_ShouldDeleteClient()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = new Client
        {
            Id = clientId,
            FirstName = "Test",
            LastName = "Client",
            Email = "test@example.com",
            Phone = "1234567890",
            Active = true,
            LastServiceDate = DateTimeOffset.UtcNow
        };
        DbContext.Clients.Add(client);
        await DbContext.SaveChangesAsync();

        // Act
        HttpResponseMessage response = await Client.DeleteAsync($"/v1/clients/{clientId}");

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteClientAsync_WhenClientDoesNotExist_ShouldBadRequest()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await Client.DeleteAsync($"/v1/clients/{id}");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion DeleteClientAsync
}
