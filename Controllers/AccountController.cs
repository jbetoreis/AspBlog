using AspBlog.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspBlog.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
    [HttpPost("v1/login")]
    public IActionResult Login([FromServices] TokenService tokenService)
    {
        var token = tokenService.GenerateToken(null);
        return Ok(token);
    }
}