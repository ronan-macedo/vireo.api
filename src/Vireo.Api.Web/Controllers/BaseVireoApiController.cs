using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vireo.Api.Core.Domain.Dtos.Responses;
using Vireo.Api.Core.Domain.Interfaces;

namespace Vireo.Api.Web.Controllers;

[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[Authorize]
public abstract class BaseVireoApiController : ControllerBase
{
    protected readonly INotifier _notifier;

    protected BaseVireoApiController(INotifier notifier)
    {
        _notifier = notifier;
    }

    protected static CreatedResult Created(object value)
    {
        var response = new CreatedResult((string?)null, value);
        return response;
    }

    protected NotFoundObjectResult NotFound(string entity)
    {
        return base.NotFound(new ErrorResponse(
                "Not found.",
                [$"{entity} not found."]
            ));
    }

    protected BadRequestObjectResult BadRequest(string message)
    {
        return base.BadRequest(new ErrorResponse(
                message,
                _notifier.GetNotifications.Select(_ => _.Message)));
    }
}
