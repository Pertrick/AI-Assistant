using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using AiAssistantApi.Models;
using Newtonsoft.Json;
using System.IO;

namespace AiAssistantApi.Services.Auth;

public class AuthService
{
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<User> RegisterUser(User user){
        //save to file
        
        if(!File.Exists("users.json")){
            await File.WriteAllTextAsync("users.json", "[]");
        }

        var users = await File.ReadAllTextAsync("users.json");
        
        var userList = JsonConvert.DeserializeObject<List<User>>(users) ?? new List<User>();

        if(userList.Any(u => u.Username == user.Username)){
            throw new Exception("Username already exists");
        }

        userList.Add(user);
        await File.WriteAllTextAsync("users.json", JsonConvert.SerializeObject(userList));

        //generate token
        var token = GenerateToken(user.Username);
        user.Token = token;

        return user;
    }

    public string GenerateToken(string username)
    {
        var jwtSettings = _configuration.GetSection("jwt");
        var keyValue = jwtSettings["key"] ?? throw new InvalidOperationException("JWT key is not configured");
        var issuer = jwtSettings["issuer"] ?? throw new InvalidOperationException("JWT issuer is not configured");
        var audience = jwtSettings["audience"] ?? throw new InvalidOperationException("JWT audience is not configured");
        var expiresInMinutes = jwtSettings["expiresInMinutes"] ?? throw new InvalidOperationException("JWT expiresInMinutes is not configured");
        
        var claims = new []{
            new Claim(ClaimTypes.NameIdentifier, username),
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(expiresInMinutes)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<User?> ValidateUser(string username, string password)
    {
        if (!File.Exists("users.json"))
        {
            return null;
        }

        var users = await File.ReadAllTextAsync("users.json");
        var userList = JsonConvert.DeserializeObject<List<User>>(users) ?? new List<User>();

        return userList.FirstOrDefault(u => u.Username == username && u.Password == password);
    }
}