using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using OfficeProcurement.Infrastructure.Common;

namespace OfficeProcurement.Infrastructure.Providers;

/// <summary>
/// Провайдер информации о текущем пользователе из JWT-токена.
/// </summary>
public class IdentityProvider : IIdentityProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Инициирует новый экземпляр <see cref="IdentityProvider"/>.
    /// </summary>
    public IdentityProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc/>
    public string Name =>
        _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anonymous";

    /// <summary>
    /// ID текущего пользователя из JWT-токена.
    /// </summary>
    public Guid UserId =>
        Guid.TryParse(
            _httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value,
            out var id)
            ? id
            : Guid.Empty;
    /// <summary>
    /// Роль текущего пользователя из JWT-токена.
    /// </summary>
    public string Role =>
        _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value ?? "User";
}
