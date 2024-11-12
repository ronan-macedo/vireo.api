using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Vireo.Api.Tests.Helpers.Integration;

public class DummyPolicyEvaluator : IPolicyEvaluator
{
    public Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
    {
        var principal = new ClaimsPrincipal();
        principal.AddIdentity(new ClaimsIdentity([new Claim(
            ClaimTypes.Name,
            "TestUser")], "TestScheme"));
        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, "TestScheme")));
    }

    public Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy,
        AuthenticateResult authenticationResult,
        HttpContext context,
        object? resource)
    {
        return Task.FromResult(PolicyAuthorizationResult.Success());
    }
}
