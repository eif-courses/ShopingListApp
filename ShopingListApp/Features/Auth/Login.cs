using System.Security.Claims;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Identity;

namespace ShopingListApp.Features.Auth;

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class Login : Endpoint<LoginRequest>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public Login(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }


    public override void Configure()
    {
        Post("/auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(req.Email);
        if (user == null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var result = await _signInManager.PasswordSignInAsync(user, req.Password, true, false);
        if (!result.Succeeded)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


        await CookieAuth.SignInAsync(u =>
            {
                u.Claims.AddRange(claims);
                u.Roles.AddRange(roles);
            }
        );
        
        
        await SendOkAsync(ct);
    }
}