using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }
    
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation($"Login attempt for user: {request.Username}");

        var user = _authService.Authenticate(request.Username, request.Password);

        if (user == null)
        {
            _logger.LogWarning($"Failed login attempt for user: {request.Username}");
            return Unauthorized(new { message = "Invalid username or password" });
        }

        _logger.LogInformation($"User {request.Username} logged in successfully");

        return Ok(new LoginResponse
        {
            Message = "Login successful",
            User = new UserDto
            {
                Username = user.Username,
                Role = user.Role
            }
        });
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    public string Message { get; set; }
    public UserDto User { get; set; }
}

public class UserDto
{
    public string Username { get; set; }
    public string Role { get; set; }
}