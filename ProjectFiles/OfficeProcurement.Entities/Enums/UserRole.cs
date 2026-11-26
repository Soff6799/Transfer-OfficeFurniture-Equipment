namespace OfficeProcurement.Entities.Enums;

/// <summary>
/// Роль пользователя системы
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Администратор — полный доступ, управление справочниками и пользователями
    /// </summary>
    Admin = 1,

    /// <summary>
    /// Менеджер — работа с договорами и актами
    /// </summary>
    Manager = 2
}
