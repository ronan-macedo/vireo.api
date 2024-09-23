using AutoFixture;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Vireo.Api.Core.Domain.Dtos.Requests;
using Vireo.Api.Core.Domain.Dtos.Responses;
using Vireo.Api.Core.Domain.Interfaces;
using Vireo.Api.Core.Domain.Interfaces.Services;
using Vireo.Api.Core.Notifications;
using Vireo.Api.Tests.Helpers.Mappings;
using Vireo.Api.Web.Controllers;

namespace Vireo.Api.Tests.Web.Controllers;

[Trait(TestTraits.UnitTest, TestTraits.ControllerTest)]
public class ClientsControllerTests
{
    private readonly IClientService _clientService = Substitute.For<IClientService>();

    private readonly INotifier _notifier = Substitute.For<INotifier>();

    private readonly ILogger<ClientsController> _logger = Substitute.For<ILogger<ClientsController>>();

    private readonly IValidator<CreateClientRequest> _createClientRequestValidator = Substitute.For<IValidator<CreateClientRequest>>();

    private readonly IValidator<UpdateClientRequest> _updateClientRequestValidator = Substitute.For<IValidator<UpdateClientRequest>>();

    private readonly IFixture _fixture = new Fixture();

    private readonly ClientsController _sut;

    public ClientsControllerTests()
    {
        _sut = new ClientsController(
            _clientService,
            _notifier,
            _logger,
            _updateClientRequestValidator,
            _createClientRequestValidator);
    }

    #region GetClientsAsync

    [Fact]
    public async Task GetClientsAsync_ReturnsOkResult_WithClients()
    {
        // Arrange
        PaginatedResult<GetClientResponse> clients = _fixture.Create<PaginatedResult<GetClientResponse>>();
        _clientService.GetClientsAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(clients);

        // Act
        IActionResult result = await _sut.GetClientsAsync(1, 10);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(clients, okResult.Value);
    }

    [Fact]
    public async Task GetClientsAsync_ReturnsOkResult_WithoutClients()
    {
        // Arrange
        var clients = new PaginatedResult<GetClientResponse>([], 0, 1, 1);
        _clientService.GetClientsAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(clients);

        // Act
        IActionResult result = await _sut.GetClientsAsync(1, 10);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(clients, okResult.Value);
    }

    [Fact]
    public async Task GetClientsAsync_ThrowsException_WhenServiceFails()
    {
        // Arrange
        _clientService.GetClientsAsync(Arg.Any<int>(), Arg.Any<int>()).Throws(new Exception("Exception test."));

        // Act
        IActionResult result = await _sut.GetClientsAsync(1, 10);

        // Assert
        ObjectResult objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    #endregion GetClientsAsync

    #region GetClientByIdAsync

    [Fact]
    public async Task GetClientByIdAsync_ReturnsClient_WhenClientDoesExist()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        GetClientResponse client = _fixture.Create<GetClientResponse>();
        _clientService.GetClientByIdAsync(Arg.Any<Guid>()).Returns(client);

        // Act
        IActionResult result = await _sut.GetClientByIdAsync(clientId);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(client, okResult.Value);
    }

    [Fact]
    public async Task GetClientByIdAsync_ReturnsNotFound_WhenClientDoesNotExist()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        _clientService.GetClientByIdAsync(Arg.Any<Guid>()).Returns((GetClientResponse?)null);

        // Act
        IActionResult result = await _sut.GetClientByIdAsync(clientId);

        // Assert
        NotFoundObjectResult notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetClientByIdAsync_ThrowsException_WhenServiceFails()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        _clientService.GetClientByIdAsync(Arg.Any<Guid>()).Throws(new Exception("Exception test."));

        // Act
        IActionResult result = await _sut.GetClientByIdAsync(clientId);

        // Assert
        ObjectResult objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    #endregion GetClientByIdAsync

    #region CreateClientAsync

    [Fact]
    public async Task CreateClientAsync_ReturnsCreated_WhenCreationIsSuccessful()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        CreateClientRequest request = _fixture.Create<CreateClientRequest>();
        var response = request.ToGetClientResponse(clientId);
        var validationResult = new ValidationResult();
        _createClientRequestValidator.Validate(Arg.Any<CreateClientRequest>()).Returns(validationResult);
        _clientService.AddClientAsync(Arg.Any<CreateClientRequest>()).Returns(clientId);
        _clientService.GetClientByIdAsync(Arg.Any<Guid>()).Returns(response);

        // Act
        IActionResult result = await _sut.CreateClientAsync(request);

        // Assert
        ObjectResult objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
        Assert.Equal(response, objectResult.Value);
    }

    [Fact]
    public async Task CreateClientAsync_ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        CreateClientRequest request = _fixture.Create<CreateClientRequest>();
        ValidationResult validationResult = _fixture.Create<ValidationResult>();
        _createClientRequestValidator.Validate(Arg.Any<CreateClientRequest>()).Returns(validationResult);

        // Act
        IActionResult result = await _sut.CreateClientAsync(request);

        // Assert
        BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task CreateClientAsync_ReturnsBadRequest_WhenHasNotification()
    {
        // Arrange
        CreateClientRequest request = _fixture.Create<CreateClientRequest>();
        var validationResult = new ValidationResult();
        _createClientRequestValidator.Validate(Arg.Any<CreateClientRequest>()).Returns(validationResult);
        _notifier.HasNotification.Returns(true);
        _notifier.GetNotifications.Returns(_fixture.Create<ICollection<Notification>>());

        // Act
        IActionResult result = await _sut.CreateClientAsync(request);

        // Assert
        BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task CreateClientAsync_ReturnsProblem_WhenClientNotFound()
    {
        // Arrange
        CreateClientRequest request = _fixture.Create<CreateClientRequest>();
        var validationResult = new ValidationResult();
        _createClientRequestValidator.Validate(Arg.Any<CreateClientRequest>()).Returns(validationResult);
        _clientService.GetClientByIdAsync(Arg.Any<Guid>()).Returns((GetClientResponse?)null);

        // Act
        IActionResult result = await _sut.CreateClientAsync(request);

        // Assert
        ObjectResult objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Fact]
    public async Task CreateClientAsync_ThrowsException_WhenServiceFails()
    {
        // Arrange
        CreateClientRequest request = _fixture.Create<CreateClientRequest>();
        var validationResult = new ValidationResult();
        _createClientRequestValidator.Validate(Arg.Any<CreateClientRequest>()).Returns(validationResult);
        _clientService.AddClientAsync(Arg.Any<CreateClientRequest>()).Throws(new Exception("Exception test."));

        // Act
        IActionResult result = await _sut.CreateClientAsync(request);

        // Assert
        ObjectResult objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    #endregion CreateClientAsync

    #region UpdateClientAsync

    [Fact]
    public async Task UpdateClientAsync_ReturnsNoContent_WhenUpdateIsSuccessful()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        UpdateClientRequest request = _fixture.Create<UpdateClientRequest>();
        var validationResult = new ValidationResult();
        _updateClientRequestValidator.Validate(Arg.Any<UpdateClientRequest>()).Returns(validationResult);
        _notifier.HasNotification.Returns(false);

        // Act
        IActionResult result = await _sut.UpdateClientAsync(clientId, request);

        // Assert
        NoContentResult noContentResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
    }

    [Fact]
    public async Task UpdateClientAsync_ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        UpdateClientRequest request = _fixture.Create<UpdateClientRequest>();
        ValidationResult validationResult = _fixture.Create<ValidationResult>();
        _updateClientRequestValidator.Validate(Arg.Any<UpdateClientRequest>()).Returns(validationResult);

        // Act
        IActionResult result = await _sut.UpdateClientAsync(clientId, request);

        // Assert
        BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task UpdateClientAsync_ReturnsBadRequest_WhenHasNotification()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        UpdateClientRequest request = _fixture.Create<UpdateClientRequest>();
        var validationResult = new ValidationResult();
        _updateClientRequestValidator.Validate(Arg.Any<UpdateClientRequest>()).Returns(validationResult);
        _notifier.HasNotification.Returns(true);
        _notifier.GetNotifications.Returns(_fixture.Create<ICollection<Notification>>());

        // Act
        IActionResult result = await _sut.UpdateClientAsync(clientId, request);

        // Assert
        BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task UpdateClientAsync_ThrowsException_WhenServiceFails()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        UpdateClientRequest request = _fixture.Create<UpdateClientRequest>();
        var validationResult = new ValidationResult();
        _updateClientRequestValidator.Validate(Arg.Any<UpdateClientRequest>()).Returns(validationResult);
        _clientService.UpdateClientAsync(Arg.Any<UpdateClientRequest>(), Arg.Any<Guid>()).Throws(new Exception("Exception test."));

        // Act
        IActionResult result = await _sut.UpdateClientAsync(clientId, request);

        // Assert
        ObjectResult objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    #endregion UpdateClientAsync

    #region DeleteClientAsync

    [Fact]
    public async Task DeleteClientAsync_ReturnsNoContent_WhenDeletionIsSuccessful()
    {
        // Arrange
        var clientId = Guid.NewGuid();

        // Act
        IActionResult result = await _sut.DeleteClientAsync(clientId);

        // Assert
        NoContentResult noContentResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
    }

    [Fact]
    public async Task DeleteClientAsync_ReturnsBadRequest_WhenHasNotification()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        _notifier.HasNotification.Returns(true);
        _notifier.GetNotifications.Returns(_fixture.Create<ICollection<Notification>>());

        // Act
        IActionResult result = await _sut.DeleteClientAsync(clientId);

        // Assert
        BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task DeleteClientAsync_ThrowsException_WhenServiceFails()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        _clientService.DeleteClientAsync(Arg.Any<Guid>()).Throws(new Exception("Exception test."));

        // Act
        IActionResult result = await _sut.DeleteClientAsync(clientId);

        // Assert
        ObjectResult objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    #endregion DeleteClientAsync

    #region SearchClientsAsync

    [Fact]
    public async Task SearchClientsAsync_ReturnsOkResult_WithClients()
    {
        // Arrange
        PaginatedResult<GetClientResponse> clients = _fixture.Create<PaginatedResult<GetClientResponse>>();
        _clientService.SearchClientsAsync(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<int>(),
            Arg.Any<int>())
            .Returns(clients);

        // Act
        IActionResult result = await _sut.SearchClientsAsync("John", "Doe", 1, 10);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(clients, okResult.Value);
    }

    [Fact]
    public async Task SearchClientsAsync_ReturnsOkResult_WithoutClients()
    {
        // Arrange
        var clients = new PaginatedResult<GetClientResponse>([], 0, 1, 1);
        _clientService.SearchClientsAsync(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<int>(),
            Arg.Any<int>())
            .Returns(clients);

        // Act
        IActionResult result = await _sut.SearchClientsAsync("John", "Doe", 1, 10);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(clients, okResult.Value);
    }

    [Fact]
    public async Task SearchClientsAsync_ThrowsException_WhenServiceFails()
    {
        // Arrange
        _clientService.SearchClientsAsync(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<int>(),
            Arg.Any<int>())
            .Throws(new Exception("Exception test."));

        // Act
        IActionResult result = await _sut.SearchClientsAsync("John", "Doe", 1, 10);

        // Assert
        ObjectResult objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    #endregion SearchClientsAsync
}
