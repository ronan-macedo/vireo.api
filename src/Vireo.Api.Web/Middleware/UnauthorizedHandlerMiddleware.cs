using System.Text.Json;
using Vireo.Api.Core.Domain.Dtos.Responses;

namespace Vireo.Api.Web.Middleware;

internal class UnauthorizedHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public UnauthorizedHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        {
            var response = new ErrorResponse(
                "Unauthorized",
                ["You are not authorized to access this resource."]
            );

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
