namespace OfficeProcurement.Entities;

/// <summary>
/// Сущность поставщик
/// </summary>
public class Supplier
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
    /// КПП
    /// </summary>
    public string? Kpp  { get; set; }

    /// <summary>
    /// Юридический адрес
    /// </summary>
    public string LegalAddress { get; set; } = string.Empty;

    /// <summary>
    /// Фактический адрес (почтовый)
    /// </summary>
    public string? ActualAddress { get; set; }

    /// <summary>
    /// Электронная почта
    /// </summary>
    public string ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// номер телефона
    /// </summary>
    public string ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// Флаг активности
    /// </summary>
    public bool IsActive { get; set; } = true;
}
