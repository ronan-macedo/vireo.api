using Vireo.Api.Web.DependencyInjection.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

WebApplication app = builder.Build();

app.ConfigureWebApplication();

app.Run();