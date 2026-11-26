namespace OfficeProcurement.Services.Contracts.Models;

/// <summary>
/// Запрос на аутентификацию
/// </summary>
public class LoginRequestModel
{
    /// <summary>
    /// Логин пользователя
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Пароль пользователя
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
