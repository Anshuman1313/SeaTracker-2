namespace Assiginment.Controllers;
using Assiginment.DTO;
using Assiginment.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var result = await _userService.AuthenticateAsync(request);
        if (result == null)
            return Unauthorized(new { msg = "Invalid credentials" });

        return Ok(new { authToken = result.Token, code = "200", msg = "Login successful" });
    }
}

