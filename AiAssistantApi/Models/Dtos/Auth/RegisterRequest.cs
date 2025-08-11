namespace AiAssistantApi.Models.Dtos.Auth;

public class RegisterRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
}