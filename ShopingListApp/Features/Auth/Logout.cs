using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ShopingListApp.Features.Auth;

public class Logout : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("/auth/logout");
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await CookieAuth.SignOutAsync();
        await SendAsync("Sign Out Successfully!", cancellation: ct);
    }
}