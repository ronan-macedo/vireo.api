using AutoFixture;
using NSubstitute;
using Vireo.Api.Core.Domain.Dtos.Requests;
using Vireo.Api.Core.Domain.Dtos.Responses;
using Vireo.Api.Core.Domain.Entities;
using Vireo.Api.Core.Domain.Interfaces;
using Vireo.Api.Core.Domain.Interfaces.Repositories;
using Vireo.Api.Core.Notifications;
using Vireo.Api.Core.Services;

namespace Vireo.Api.Tests.Core.Services;

[Trait(TestTraits.UnitTest, TestTraits.ServiceTest)]
public class ClientServiceTests
{
    private readonly IClientRepository _clientRepository = Substitute.For<IClientRepository>();

    private readonly INotifier _notifier = Substitute.For<INotifier>();

    private readonly IFixture _fixture = new Fixture();

    private readonly ClientService _sut;

    public ClientServiceTests()
    {
        _sut = new ClientService(_clientRepository, _notifier);
    }

    #region AddClientAsync

    [Fact]
    public async Task AddClientAsync_WhenClientRepositoryAddAsyncReturnsTrue_ReturnsClientId()
    {
        // Arrange
        CreateClientRequest createClientRequest = _fixture.Create<CreateClientRequest>();
        _clientRepository.AddAsync(Arg.Any<Client>()).Returns(true);

        // Act
        Guid result = await _sut.AddClientAsync(createClientRequest);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
    }

    [Fact]
    public async Task AddClientAsync_WhenClientRepositoryAddAsyncReturnsFalse_AddsNotification()
    {
        // Arrange
        CreateClientRequest createClientRequest = _fixture.Create<CreateClientRequest>();
        _clientRepository.AddAsync(Arg.Any<Client>()).Returns(false);

        // Act
        Guid result = await _sut.AddClientAsync(createClientRequest);

        // Assert
        Assert.Equal(Guid.Empty, result);
        _notifier.Received(1).AddNotification(Arg.Any<Notification>());
    }

    #endregion AddClientAsync

    #region DeleteClientAsync

    [Fact]
    public async Task DeleteClientAsync_WhenClientRepositoryDeleteAsyncReturnsTrue_DoesNotAddNotification()
    {
        // Arrange
        Guid id = _fixture.Create<Guid>();
        _clientRepository.DeleteAsync(id).Returns(true);

        // Act
        await _sut.DeleteClientAsync(id);

        // Assert
        _notifier.DidNotReceive().AddNotification(Arg.Any<Notification>());
    }

    [Fact]
    public async Task DeleteClientAsync_WhenClientRepositoryDeleteAsyncReturnsFalse_AddsNotification()
    {
        // Arrange
        Guid id = _fixture.Create<Guid>();
        _clientRepository.DeleteAsync(id).Returns(false);

        // Act
        await _sut.DeleteClientAsync(id);

        // Assert
        _notifier.Received(1).AddNotification(Arg.Any<Notification>());
    }

    #endregion DeleteClientAsync

    #region GetClientByIdAsync

    [Fact]
    public async Task GetClientByIdAsync_WhenClientRepositoryGetByIdAsyncReturnsClient_ReturnsGetClientResponse()
    {
        // Arrange
        Guid id = _fixture.Create<Guid>();
        Client client = _fixture.Create<Client>();
        _clientRepository.GetByIdAsync(id).Returns(client);

        // Act
        GetClientResponse? result = await _sut.GetClientByIdAsync(id);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetClientByIdAsync_WhenClientRepositoryGetByIdAsyncReturnsNull_ReturnsNull()
    {
        // Arrange
        Guid id = _fixture.Create<Guid>();
        _clientRepository.GetByIdAsync(id).Returns((Client?)null);

        // Act
        GetClientResponse? result = await _sut.GetClientByIdAsync(id);

        // Assert
        Assert.Null(result);
    }

    #endregion GetClientByIdAsync

    #region GetClientsAsync

    [Fact]
    public async Task GetClientsAsync_WhenClientRepositoryGetClientsAsync_ReturnsPaginatedResultOfGetClientResponse()
    {
        // Arrange
        PaginatedRequest paginatedRequest = _fixture.Create<PaginatedRequest>();
        PaginatedResult<Client> paginatedResult = _fixture.Create<PaginatedResult<Client>>();
        _clientRepository.GetClientsAsync(paginatedRequest.PageNumber, paginatedRequest.PageSize).Returns(paginatedResult);

        // Act
        PaginatedResult<GetClientResponse> result = await _sut.GetClientsAsync(paginatedRequest);

        // Assert
        Assert.Equal(paginatedResult.TotalCount, result.TotalCount);
        Assert.Equal(paginatedResult.Items.Count(), result.Items.Count());
    }

    #endregion GetClientsAsync

    #region UpdateClientAsync

    [Fact]
    public async Task UpdateClientAsync_WhenClientRepositoryUpdateAsyncReturnsTrue_DoesNotAddNotification()
    {
        // Arrange
        UpdateClientRequest updateClientRequest = _fixture.Create<UpdateClientRequest>();
        Guid id = _fixture.Create<Guid>();
        _clientRepository.UpdateAsync(Arg.Any<Client>()).Returns(true);

        // Act
        await _sut.UpdateClientAsync(updateClientRequest, id);

        // Assert
        _notifier.DidNotReceive().AddNotification(Arg.Any<Notification>());
    }

    [Fact]
    public async Task UpdateClientAsync_WhenClientRepositoryUpdateAsyncReturnsFalse_AddsNotification()
    {
        // Arrange
        UpdateClientRequest updateClientRequest = _fixture.Create<UpdateClientRequest>();
        Guid id = _fixture.Create<Guid>();
        _clientRepository.UpdateAsync(Arg.Any<Client>()).Returns(false);

        // Act
        await _sut.UpdateClientAsync(updateClientRequest, id);

        // Assert
        _notifier.Received(1).AddNotification(Arg.Any<Notification>());
    }

    #endregion UpdateClientAsync

    #region SearchClientsAsync

    [Fact]
    public async Task SearchClientsAsync_WhenClientRepositorySearchClientsAsync_ReturnsPaginatedResultOfGetClientResponse()
    {
        // Arrange
        SearchClientRequest searchClientRequest = _fixture.Create<SearchClientRequest>();
        PaginatedResult<Client> paginatedResult = _fixture.Create<PaginatedResult<Client>>();
        _clientRepository.SearchClientsAsync(
            searchClientRequest.FirstName,
            searchClientRequest.LastName,
            searchClientRequest.PageNumber,
            searchClientRequest.PageSize).Returns(paginatedResult);

        // Act
        PaginatedResult<GetClientResponse> result = await _sut.SearchClientsAsync(searchClientRequest);

        // Assert
        Assert.Equal(paginatedResult.TotalCount, result.TotalCount);
        Assert.Equal(paginatedResult.Items.Count(), result.Items.Count());
    }

    #endregion SearchClientsAsync
}
