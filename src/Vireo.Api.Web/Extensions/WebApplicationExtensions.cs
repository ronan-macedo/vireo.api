using Vireo.Api.Web.Middleware;

namespace Vireo.Api.Web.Extensions;

internal static class WebApplicationExtensions
{
    internal static void ConfigureWebApplication(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vireo API v1"));
        }

        app.UseHttpsRedirection();

        app.UseMiddleware<UnauthorizedHandlerMiddleware>();
        app.UseMiddleware<SecureHeadersMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.UseCors();

        app.MapHealthChecks("/health");
    }
}
