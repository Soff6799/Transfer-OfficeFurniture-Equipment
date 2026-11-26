using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Services.Contracts.Interface;

/// <summary>
/// Сервис аутентификации.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Аутентифицирует пользователя и возвращает JWT-токен.
    /// </summary>
    Task<LoginResponseModel?> LoginAsync(string login, string password);
}
