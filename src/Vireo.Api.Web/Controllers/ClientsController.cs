using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vireo.Api.Core.Domain.Dtos.Requests;
using Vireo.Api.Core.Domain.Dtos.Responses;
using Vireo.Api.Core.Domain.Entities;
using Vireo.Api.Core.Domain.Interfaces;
using Vireo.Api.Core.Domain.Interfaces.Services;
using Vireo.Api.Web.Attributes;

namespace Vireo.Api.Web.Controllers;

[Route("v1/clients")]
public class ClientsController : BaseVireoApiController
{
    private readonly IClientService _clientService;

    private readonly ILogger<ClientsController> _logger;

    private readonly IValidationService _validationService;

    public ClientsController(
        IClientService clientService,
        INotifier notifier,
        ILogger<ClientsController> logger,
        IValidationService validationService) : base(notifier)
    {
        _clientService = clientService;
        _logger = logger;
        _validationService = validationService;
    }

    /// <summary>
    /// Retrieves a paginated list of clients.
    /// </summary>
    /// <param name="request">The object with the paging parameters.</param>
    /// <returns>A paginated list of clients.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<GetClientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetClientsAsync([FromQuery] PaginatedRequest request)
    {
        try
        {
            _logger.LogDebug("Starting {Method} with fallowing parameters: {@Request}", nameof(GetClientsAsync), request);
            PaginatedResult<GetClientResponse> clients = await _clientService.GetClientsAsync(request);

            return Ok(clients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error in {Method} in {Controller}.", nameof(GetClientsAsync), nameof(ClientsController));
            return Problem("An error occurred.");
        }
        finally
        {
            _logger.LogDebug("Ending {Method}.", nameof(GetClientsAsync));
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
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetClientByIdAsync([FromRoute, GuidNotEmpty] Guid id)
    {
        try
        {
            _logger.LogDebug("Starting {Method} with fallowing parameters: {@Id}", nameof(GetClientByIdAsync), id);
            GetClientResponse? client = await _clientService.GetClientByIdAsync(id);

            return client is null ? NotFound(nameof(Client)) : Ok(client);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error in {Method} in {Controller}.", nameof(GetClientByIdAsync), nameof(ClientsController));
            return Problem("An error occurred.");
        }
        finally
        {
            _logger.LogDebug("Ending {Method}.", nameof(GetClientByIdAsync));
        }
    }

    /// <summary>
    /// Searches for clients based on the provided criteria.
    /// </summary>
    /// <param name="request">The object with the client query.</param>
    /// <returns>A paginated list of clients matching the search criteria.</returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(PaginatedResult<GetClientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchClientsAsync([FromQuery] SearchClientRequest request)
    {
        try
        {
            _logger.LogDebug("Starting {Method} with fallowing parameters: {@Request}", nameof(SearchClientsAsync), request);

            PaginatedResult<GetClientResponse> clients = await _clientService.SearchClientsAsync(request);

            return Ok(clients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error in {Method} in {Controller}.", nameof(SearchClientsAsync), nameof(ClientsController));
            return Problem("An error occurred.");
        }
        finally
        {
            _logger.LogDebug("Ending {Method}.", nameof(SearchClientsAsync));
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
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateClientAsync([FromBody] CreateClientRequest request)
    {
        try
        {
            _logger.LogDebug("Starting {Method} with fallowing parameters: {@Request}", nameof(CreateClientAsync), request);

            _validationService.Validate(request);
            if (!_validationService.IsValid)
            {
                return BadRequest(_validationService.GetErrorResponse);
            }

            Guid clientId = await _clientService.AddClientAsync(request);

            if (_notifier.HasNotification)
            {
                return BadRequest($"{nameof(Client)} creation failed");
            }

            GetClientResponse? client = await _clientService.GetClientByIdAsync(clientId);

            if (client == null)
            {
                _logger.LogError("{Entity} failed to return newly created value.", nameof(Client));
                return Problem($"{nameof(Client)} failed to return newly created value.");
            }

            return Created(client);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error in {Method} in {Controller}.", nameof(CreateClientAsync), nameof(ClientsController));
            return Problem("An error occurred.");
        }
        finally
        {
            _logger.LogDebug("Ending {Method}.", nameof(CreateClientAsync));
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
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateClientAsync(
        [FromRoute, GuidNotEmpty] Guid id,
        [FromBody] UpdateClientRequest request)
    {
        try
        {
            _logger.LogDebug("Starting {Method} with fallowing parameters: {@Request}, {@Id}", nameof(UpdateClientAsync), request, id);

            _validationService.Validate(request);
            if (!_validationService.IsValid)
            {
                return BadRequest(_validationService.GetErrorResponse);
            }

            await _clientService.UpdateClientAsync(request, id);

            return _notifier.HasNotification
                ? BadRequest($"{nameof(Client)} update failed")
                : NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error in {Method} in {Controller}.", nameof(UpdateClientAsync), nameof(ClientsController));
            return Problem("An error occurred.");
        }
        finally
        {
            _logger.LogDebug("Ending {Method}.", nameof(UpdateClientAsync));
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
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteClientAsync([FromRoute, GuidNotEmpty] Guid id)
    {
        try
        {
            _logger.LogDebug("Starting {Method} with fallowing parameters: {@Id}", nameof(DeleteClientAsync), id);

            await _clientService.DeleteClientAsync(id);

            return _notifier.HasNotification
                ? BadRequest($"{nameof(Client)} delete failed")
                : NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error in {Method} in {Controller}.", nameof(DeleteClientAsync), nameof(ClientsController));
            return Problem("An error occurred.");
        }
        finally
        {
            _logger.LogDebug("Ending {Method}.", nameof(DeleteClientAsync));
        }
    }
}
