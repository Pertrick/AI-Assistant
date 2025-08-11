using Microsoft.AspNetCore.Mvc;
using AiAssistantApi.Models.Dtos.Auth;
using AiAssistantApi.Services.Auth;
using AiAssistantApi.Models;
namespace AiAssistantApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if(string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest(new { message = "Username and password are required" });
        }

        try
        {
            var user = await _authService.ValidateUser(request.Username, request.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var token = _authService.GenerateToken(user.Username);
            return Ok(new { token, user });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try{
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                Password = request.Password
            };
            
            var registeredUser = await _authService.RegisterUser(user);
            return Ok(new { user = registeredUser, message = "Register successful" });
        }
        catch(Exception ex){
            return BadRequest(new { message = ex.Message });
        }
    }
}