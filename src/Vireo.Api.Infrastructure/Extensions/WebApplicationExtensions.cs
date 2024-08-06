using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Vireo.Api.Infrastructure.Extensions;

public static class WebApplicationExtensions
{
    public static void ConfigureWebApplication(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
    }
}
