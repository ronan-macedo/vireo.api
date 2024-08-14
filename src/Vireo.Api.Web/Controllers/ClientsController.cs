using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Vireo.Api.Core.Domain.Dtos.Requests;
using Vireo.Api.Core.Domain.Dtos.Responses;
using Vireo.Api.Core.Domain.Interfaces;
using Vireo.Api.Core.Domain.Interfaces.Services;
using Vireo.Api.Web.Attribute;

namespace Vireo.Api.Web.Controllers;

[Route("v1/clients")]
[Consumes("application/json")]
[Produces("application/json")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    private readonly INotifier _notifier;

    private readonly ILogger<ClientsController> _logger;

    private readonly IValidator<CreateClientRequest> _createClientRequestValidator;

    private readonly IValidator<UpdateClientRequest> _updateClientRequestValidator;

    public ClientsController(
        IClientService clientService,
        INotifier notifier,
        ILogger<ClientsController> logger,
        IValidator<UpdateClientRequest> updateClientRequestValidator,
        IValidator<CreateClientRequest> createClientRequestValidator)
    {
        _clientService = clientService;
        _notifier = notifier;
        _logger = logger;
        _updateClientRequestValidator = updateClientRequestValidator;
        _createClientRequestValidator = createClientRequestValidator;
    }

    /// <summary>
    /// Retrieves a paginated list of clients.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of clients per page.</param>
    /// <returns>A paginated list of clients.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<GetClientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetClientsAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            _logger.LogDebug("Starting get clients request");
            PaginatedResult<GetClientResponse> clients = await _clientService.GetClientsAsync(pageNumber, pageSize);

            return Ok(clients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting clients.");

            return Problem("Um erro ocorreu ao buscar por clientes.");
        }
        finally
        {
            _logger.LogDebug("Ending get clients request");
        }
    }

    /// <summary>
    /// Retrieves a client by its ID.
    /// </summary>
    /// <param name="id">The ID of the client to retrieve.</param>
    /// <returns>The client with the specified ID.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetClientResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetClientByIdAsync([FromRoute, GuidNotEmpty] Guid id)
    {
        try
        {
            _logger.LogDebug("Starting get client by id: {id}", id);
            GetClientResponse? client = await _clientService.GetClientByIdAsync(id);

            return client is null ? NotFound(new ErrorResponse(
                "Not found.",
                ["Cliente não encontrado."]
            )) : Ok(client);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting client by id.");

            return Problem("Um erro ocorreu ao buscar cliente por id.");
        }
        finally
        {
            _logger.LogDebug("Ending get client by id request");
        }
    }

    /// <summary>
    /// Creates a new client.
    /// </summary>
    /// <param name="request">The request containing the client details.</param>
    /// <returns>The result of the client creation.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(GetClientResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateClientAsync([FromBody] CreateClientRequest request)
    {
        try
        {
            _logger.LogDebug("Starting create client request with parameters: {request}", request);

            ValidationResult validate = _createClientRequestValidator.Validate(request);
            if (!validate.IsValid)
            {
                return BadRequest(
                    new ErrorResponse("Validation error.",
                    validate.Errors.Select(_ => _.ErrorMessage)));
            }

            Guid clientId = await _clientService.AddClientAsync(request);

            if (_notifier.HasNotification)
            {
                return BadRequest(new ErrorResponse(
               "Client creation failed",
               _notifier.Notifications.Select(n => n.Message)));
            }

            GetClientResponse? client = await _clientService.GetClientByIdAsync(clientId);

            if (client == null)
            {
                _logger.LogError("Failed to retrieve the newly created client with id: {id}", clientId);
                return Problem("Falha ao retornar cliente recém criado.");
            }

            return new ObjectResult(client) { StatusCode = StatusCodes.Status201Created };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating client.");

            return Problem("Um erro ocorreu ao adicionar um novo cliente.");
        }
        finally
        {
            _logger.LogDebug("Ending create client request");
        }
    }

    /// <summary>
    /// Updates an existing client.
    /// </summary>
    /// <param name="id">The ID of the client to update.</param>
    /// <param name="request">The request containing the updated client details.</param>
    /// <returns>The result of the client update.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateClientAsync(
        [FromRoute, GuidNotEmpty] Guid id,
        [FromBody] UpdateClientRequest request)
    {
        try
        {
            _logger.LogDebug("Starting update client request with parameters: {request}", request);

            ValidationResult validate = _updateClientRequestValidator.Validate(request);
            if (!validate.IsValid)
            {
                return BadRequest(
                    new ErrorResponse("Validation error.",
                    validate.Errors.Select(_ => _.ErrorMessage)));
            }

            await _clientService.UpdateClientAsync(request, id);

            return _notifier.HasNotification
                ? BadRequest(new ErrorResponse(
                    "Client update failed.",
                    _notifier.Notifications.Select(_ => _.Message)))
                : NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating client.");

            return Problem("Um erro ocorreu ao modificar um cliente.");
        }
        finally
        {
            _logger.LogDebug("Ending update client request");
        }
    }

    /// <summary>
    /// Deletes a client by its ID.
    /// </summary>
    /// <param name="id">The ID of the client to delete.</param>
    /// <returns>The result of the client deletion.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteClientAsync([FromRoute, GuidNotEmpty] Guid id)
    {
        try
        {
            _logger.LogDebug("Starting delete client by id: {id}", id);

            await _clientService.DeleteClientAsync(id);

            return _notifier.HasNotification
                ? BadRequest(new ErrorResponse(
                    "Client delete failed.",
                    _notifier.Notifications.Select(_ => _.Message)))
                : NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting client.");

            return Problem("Um erro ocorreu ao deletar um cliente.");
        }
        finally
        {
            _logger.LogDebug("Ending delete client request");
        }
    }

    /// <summary>
    /// Searches for clients based on the provided criteria.
    /// </summary>
    /// <param name="name">The name of the client to search for.</param>
    /// <param name="lastName">The last name of the client to search for.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of clients per page.</param>
    /// <returns>A paginated list of clients matching the search criteria.</returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(PaginatedResult<GetClientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchClientsAsync(
        [FromQuery] string? name,
        [FromQuery] string? lastName,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            _logger.LogDebug("Starting search clients request");

            PaginatedResult<GetClientResponse> clients = await _clientService.SearchClientsAsync(
                name,
                lastName,
                pageNumber,
                pageSize);

            return Ok(clients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while searching clients.");

            return Problem("Um erro ocorreu ao buscar por clientes.");
        }
        finally
        {
            _logger.LogDebug("Ending search clients request");
        }
    }
}
