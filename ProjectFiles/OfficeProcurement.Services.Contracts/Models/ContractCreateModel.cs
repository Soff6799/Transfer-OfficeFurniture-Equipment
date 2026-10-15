using OfficeProcurement.Entities;

namespace OfficeProcurement.Services.Contracts.Models;

/// <summary>
/// Модель для создания договора
/// </summary>
public class ContractCreateModel
{
    /// <summary>
    /// Номер договора
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// Дата подписания
    /// </summary>
    public DateTimeOffset SignedAt { get; set; }

    /// <summary>
    /// Тип договора
    /// </summary>
    public ContractType Type { get; set; }
}
