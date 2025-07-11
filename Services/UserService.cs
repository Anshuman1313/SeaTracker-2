using Assiginment.DTO;
using Assiginment.Models;
using Assiginment.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class UserService : IUserService
{
    private readonly DevContext _context;
    private readonly IConfiguration _configuration;

    public UserService(DevContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto?> AuthenticateAsync(LoginRequestDto loginRequest)
    {
        var user = await _context.Users
           .FirstOrDefaultAsync(u => u.UserName == loginRequest.UserName && u.IsActive == true);

        if (user == null || user.PasswordHash != loginRequest.Password) // ideally hash check
            return null;

        var token = GenerateJwtToken(user);

        return new LoginResponseDto
        {
            UserId = user.UserId,
            UserName = user.UserName,
            Role = user.Role,
            Token = token
        };
    }
    //create user service create employee and user dto 
    //fields user name empl id 
    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("UserId", user.UserId.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
