using Microsoft.AspNetCore.Mvc;
using Vireo.Api.Core.Domain.Dtos.Responses;
using Vireo.Api.Core.Domain.Interfaces;
using Vireo.Api.Core.Domain.Interfaces.Services;

namespace Vireo.Api.Web.Controllers;

[Route("v1/clients")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    private readonly INotifier _notifier;

    private readonly ILogger<ClientsController> _logger;

    public ClientsController(
        IClientService clientService,
        INotifier notifier,
        ILogger<ClientsController> logger)
    {
        _clientService = clientService;
        _notifier = notifier;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetClientsAsync([FromQuery] int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            PaginatedResult<GetClientResponse> clients = await _clientService.GetClientsAsync(pageNumber, pageSize);

            return Ok(clients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting clients.");

            return StatusCode(500, "An error occurred while getting clients.");
        }
    }
}
