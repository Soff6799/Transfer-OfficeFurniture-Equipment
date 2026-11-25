namespace OfficeProcurement.Services.Contracts.Models;

/// <summary>
/// Ответ после успешной аутентификации
/// </summary>
public class LoginResponseModel
{
    /// <summary>
    /// JWT-токен.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Логин пользователя
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Роль пользователя
    /// </summary>
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// Полное имя пользователя
    /// </summary>
    public string FullName { get; set; } = string.Empty;
}
