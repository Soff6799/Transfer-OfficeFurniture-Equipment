using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using OfficeProcurement.Infrastructure.Common;

namespace OfficeProcurement.Infrastructure.Providers;

/// <summary>
/// Провайдер информации о текущем пользователе из JWT-токена.
/// </summary>
public class IdentityProvider : IIdentityProvider
{
    private readonly IHttpContextAccessor httpContextAccessor;

    /// <summary>
    /// Инициирует новый экземпляр <see cref="IdentityProvider"/>.
    /// </summary>
    public IdentityProvider(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc/>
    public string Name =>
        httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anonymous";

    /// <summary>
    /// ID текущего пользователя из JWT-токена.
    /// </summary>
    public Guid UserId =>
        Guid.TryParse(
            httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value,
            out var id)
            ? id
            : Guid.Empty;
    /// <summary>
    /// Роль текущего пользователя из JWT-токена.
    /// </summary>
    public string Role =>
        httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value ?? "User";
}
