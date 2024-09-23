using Vireo.Api.Web.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

WebApplication app = builder.Build();

app.ConfigureWebApplication();

await app.RunAsync();
