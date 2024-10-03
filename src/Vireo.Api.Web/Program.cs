using Serilog;
using Vireo.Api.Web.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.ConfigureServices(builder.Configuration);

WebApplication app = builder.Build();

app.ConfigureWebApplication();

await app.RunAsync();

#pragma warning disable S1118 // Utility classes should not have public constructors

public partial class Program
#pragma warning restore S1118 // Utility classes should not have public constructors
{ }
