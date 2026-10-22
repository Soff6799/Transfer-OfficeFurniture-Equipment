namespace OfficeProcurement.Models.Supplier;

/// <summary>
/// Модель создания поставщика для API
/// </summary>
public class SupplierCreateApiModel
{
    /// <summary>
    /// Название компании
    /// </summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// Фамилия контактного лица
    /// </summary>
    public string ContactLastName { get; set; } = string.Empty;

    /// <summary>
    /// Имя контактного лица
    /// </summary>
    public string ContactName { get; set; } = string.Empty;

    /// <summary>
    /// Отчество контактного лица
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
    /// Номер телефона
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Флаг активности
    /// </summary>
    public bool IsActive { get; set; } = true;

}
