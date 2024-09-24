using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Vireo.Api.Core.Domain.Dtos.Responses;
using Vireo.Api.Core.Domain.Entities;
using Vireo.Api.Infrastructure.Repositories;
using Vireo.Api.Tests.Helpers.Data;

namespace Vireo.Api.Tests.Infrastructure.Repositories;

[Trait(TestTraits.UnitTest, TestTraits.RepositoryTest)]
public class ClientRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _dataFixture;

    private readonly ClientRepository _sut;

    private readonly IFixture _fixture = new Fixture();

    public ClientRepositoryTests(DatabaseFixture databaseFixture)
    {
        _dataFixture = databaseFixture;
        _dataFixture.Context.Database.EnsureDeleted();
        _dataFixture.Context.Database.EnsureCreated();
        _sut = new ClientRepository(_dataFixture.Context);
    }

    #region AddAsync

    [Fact]
    public async Task AddAsync_WhenClientIsValid_ShouldReturnTrue()
    {
        // Arrange
        Client client = _fixture.Create<Client>();

        // Act
        bool result = await _sut.AddAsync(client);

        // Assert
        Assert.True(result);
    }

    #endregion AddAsync

    #region GetClientsAsync

    [Fact]
    public async Task GetClientsAsync_WhenClientsExist_ShouldReturnClients()
    {
        // Arrange
        var clients = _fixture.CreateMany<Client>(10).ToList();
        await _dataFixture.Context.Clients.AddRangeAsync(clients);
        await _dataFixture.Context.SaveChangesAsync();

        // Act
        PaginatedResult<Client> result = await _sut.GetClientsAsync(1, 10);

        // Assert
        Assert.Equal(clients.Count, result.TotalCount);
    }

    #endregion GetClientsAsync

    #region GetClientByIdAsync

    [Fact]
    public async Task GetClientByIdAsync_WhenClientExists_ShouldReturnClient()
    {
        // Arrange
        Client client = _fixture.Create<Client>();
        await _dataFixture.Context.Clients.AddAsync(client);
        await _dataFixture.Context.SaveChangesAsync();

        // Act
        Client? result = await _sut.GetByIdAsync(client.Id);

        // Assert
        Assert.NotNull(result);
    }

    #endregion GetClientByIdAsync

    #region SearchClientsAsync

    [Fact]
    public async Task SearchClientsAsync_WhenClientsExist_ShouldReturnClient()
    {
        // Arrange
        var clients = _fixture.CreateMany<Client>().ToList();
        await _dataFixture.Context.Clients.AddRangeAsync(clients);
        await _dataFixture.Context.SaveChangesAsync();
        Client client = clients[0];

        // Act
        PaginatedResult<Client> result = await _sut.SearchClientsAsync(
            client.FirstName,
            client.LastName,
            1,
            10);

        // Assert
        Assert.Equal(client.FirstName, result.Items.First().FirstName);
        Assert.Equal(client.LastName, result.Items.First().LastName);
    }

    #endregion SearchClientsAsync

    #region UpdateAsync

    [Fact]
    public async Task UpdateAsync_WhenClientExists_ShouldReturnTrue()
    {
        // Arrange
        Client client = _fixture.Create<Client>();
        await _dataFixture.Context.Clients.AddAsync(client);
        await _dataFixture.Context.SaveChangesAsync();
        _dataFixture.Context.Entry(client).State = EntityState.Detached;

        // Act
        client.FirstName = _fixture.Create<string>();
        bool result = await _sut.UpdateAsync(client);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateAsync_WhenClientDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        Client client = _fixture.Create<Client>();

        // Act
        bool result = await _sut.UpdateAsync(client);

        // Assert
        Assert.False(result);
    }

    #endregion UpdateAsync

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_WhenClientExists_ShouldReturnTrue()
    {
        // Arrange
        Client client = _fixture.Create<Client>();
        await _dataFixture.Context.Clients.AddAsync(client);
        await _dataFixture.Context.SaveChangesAsync();
        _dataFixture.Context.Entry(client).State = EntityState.Detached;

        // Act
        bool result = await _sut.DeleteAsync(client.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_WhenClientDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        Client client = _fixture.Create<Client>();

        // Act
        bool result = await _sut.DeleteAsync(client.Id);

        // Assert
        Assert.False(result);
    }

    #endregion DeleteAsync
}
