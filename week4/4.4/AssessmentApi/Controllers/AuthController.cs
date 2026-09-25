using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace AssessmentApi.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authorization) ||
            !AuthenticationHeaderValue.TryParse(authorization, out var header) ||
            !string.Equals(header.Scheme, "Basic", StringComparison.OrdinalIgnoreCase))
        {
            return Unauthorized();
        }

        try
        {
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(header.Parameter ?? string.Empty));
            var separator = credentials.IndexOf(':');
            var username = separator >= 0 ? credentials[..separator] : string.Empty;
            var password = separator >= 0 ? credentials[(separator + 1)..] : string.Empty;

            return username == "Lawson01" && password == "Password1"
                ? Ok(new { authenticated = true })
                : Unauthorized();
        }
        catch (FormatException)
        {
            return Unauthorized();
        }
    }
}