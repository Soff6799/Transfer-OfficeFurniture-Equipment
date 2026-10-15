namespace OfficeProcurement.Services.Contracts.Models;

/// <summary>
/// Модель для создания поставщика
/// </summary>
public class SupplierCreateModel
{
    /// <summary>
    /// Название компании
    /// </summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// Фамилия
    /// </summary>
    public string ContactLastName { get; set; } = string.Empty;

    /// <summary>
    /// Имя
    /// </summary>
    public string ContactName { get; set; } = string.Empty;

    /// <summary>
    /// Отчество
    /// </summary>
    public string? ContactPatronymic { get; set; }

    /// <summary>
    /// Основания действий
    /// </summary>
    public string CeoBasis { get; set; } = string.Empty;

    /// <summary>
    /// ИНН
    /// </summary>
    public string Inn { get; set; } = string.Empty;

    /// <summary>
    /// Юридический адрес
    /// </summary>
    public string LegalAddress { get; set; } = string.Empty;

    /// <summary>
    /// Электронная почта
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// телефон
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Флаг активности
    /// </summary>
    public bool IsActive { get; set; } = true;
}
