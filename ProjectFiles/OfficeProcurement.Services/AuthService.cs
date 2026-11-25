using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Entities;
using OfficeProcurement.Services.Contracts.Interface;
using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Services;

/// <summary>
/// Сервис аутентификации
/// </summary>
public class AuthService : IAuthService
{
    private readonly IReader reader;
    private readonly IConfiguration configuration;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AuthService"/>
    /// </summary>
    public AuthService(IReader reader, IConfiguration configuration)
    {
        this.reader = reader;
        this.configuration = configuration;
    }

    /// <inheritdoc/>
    public async Task<LoginResponseModel?> LoginAsync(string login, string password)
    {
        var user = await reader.Read<User>()
            .FirstOrDefaultAsync(x => x.Login == login);

        if (user == null || !VerifyPassword(password, user.PasswordHash))
        {
            return null;
        }

        return new LoginResponseModel
        {
            Token = GenerateJwtToken(user),
            Login = user.Login,
            Role = user.Role.ToString(),
            FullName = user.FullName
        };
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("fullName", user.FullName)
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(double.Parse(configuration["Jwt:ExpirationInHours"]!)),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        var hash = Convert.ToBase64String(bytes);
        return hash == passwordHash;
    }
}


