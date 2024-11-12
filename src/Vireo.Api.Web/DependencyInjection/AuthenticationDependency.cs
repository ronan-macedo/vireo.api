using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Vireo.Api.Core.Configurations;

namespace Vireo.Api.Web.DependencyInjection;

internal static class AuthenticationDependency
{
    internal static void AddAuthenticationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var authentication = new Authentication();
        configuration.Bind(nameof(Authentication), authentication);
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Authority = $"https://{authentication.Domain}/";
            options.Audience = authentication.Audience;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuerSigningKey = true
            };
        });
    }
}