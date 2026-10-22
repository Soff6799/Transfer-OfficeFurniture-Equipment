namespace OfficeProcurement.Models.Contract;

/// <summary>
/// Модель создания договора для API
/// </summary>
public class ContractCreateApiModel
{
    /// <summary>
    /// Номер договора
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// Дата договора
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public Guid ClientId { get; set; }

    /// <summary>
    /// Идентификатор поставщика
    /// </summary>
    public Guid SupplierId { get; set; }

    /// <summary>
    /// Сумма договора
    /// </summary>
    public decimal Sum { get; set; }

    /// <summary>
    /// Описание договора
    /// </summary>
    public string? Description { get; set; }

}
