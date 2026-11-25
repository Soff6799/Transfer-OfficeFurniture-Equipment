using OfficeProcurement.Dal.Contracts;
using OfficeProcurement.Entities.Enums;

namespace OfficeProcurement.Entities;

/// <summary>
/// Пользователь системы
/// </summary>
public class User : BaseAuditEntity
{
    /// <summary>
    /// Логин для входа в систему
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Хеш пароля
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Роль пользователя
    /// </summary>
    public UserRole Role { get; set; } = UserRole.Manager;

    /// <summary>
    /// Полное имя
    /// </summary>
    public string FullName { get; set; } = string.Empty;
}
