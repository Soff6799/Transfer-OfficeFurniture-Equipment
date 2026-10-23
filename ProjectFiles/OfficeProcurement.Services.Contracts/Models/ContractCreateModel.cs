using OfficeProcurement.Entities;

namespace OfficeProcurement.Services.Contracts.Models;

/// <summary>
/// Модель для создания договора
/// </summary>
public class ContractCreateModel
{
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public Guid ClientId { get; set; }

    /// <summary>
    /// Идентификатор поставщика
    /// </summary>
    public Guid SupplierId { get; set; }

    /// <summary>
    /// Номер договора
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// Дата подписания
    /// </summary>
    public DateOnly Date { get; set; }

    public decimal Sum { get; set; }

    public string? Description { get; set; }

    /// <summary>
    /// Тип договора
    /// </summary>
    public ContractType Type { get; set; }
}
