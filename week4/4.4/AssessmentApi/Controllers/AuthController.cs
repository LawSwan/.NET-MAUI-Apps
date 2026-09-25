using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace AssessmentApi.Controllers;

/// <summary>
/// Checks the login credentials sent from the app's login page using HTTP Basic Authentication.
/// </summary>
[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    /// <summary>The only user name the web service accepts.</summary>
    private const string ValidUserName = "Lawson01";

    /// <summary>The password that goes with <see cref="ValidUserName"/>.</summary>
    private const string ValidPassword = "Password1";

    /// <summary>
    /// POST api/auth/login: reads the "Authorization: Basic ..." header, decodes the
    /// base64 "username:password" value and compares it with the valid credentials.
    /// </summary>
    /// <returns>200 if the credentials match; 401 Unauthorized otherwise.</returns>
    [HttpPost("login")]
    public IActionResult Login()
    {
        // The header must be present and use the Basic scheme.
        if (!Request.Headers.TryGetValue("Authorization", out var authorization) ||
            !AuthenticationHeaderValue.TryParse(authorization, out var header) ||
            !string.Equals(header.Scheme, "Basic", StringComparison.OrdinalIgnoreCase))
        {
            return Unauthorized();
        }

        try
        {
            // Decode "username:password" and split it at the first colon.
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(header.Parameter ?? string.Empty));
            var separator = credentials.IndexOf(':');
            var username = separator >= 0 ? credentials[..separator] : string.Empty;
            var password = separator >= 0 ? credentials[(separator + 1)..] : string.Empty;

            return username == ValidUserName && password == ValidPassword
                ? Ok(new { authenticated = true })
                : Unauthorized();
        }
        catch (FormatException)
        {
            // The header value was not valid base64.
            return Unauthorized();
        }
    }
}
