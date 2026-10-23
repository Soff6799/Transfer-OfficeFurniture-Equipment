using OfficeProcurement.Dal.Contracts;

namespace OfficeProcurement.Entities;

/// <summary>
/// Сущность самого акта
/// </summary>
public class TransferAct : BaseAuditEntity
{
    /// <summary>
    /// Идентификатор договора
    /// </summary>
    public Guid ContractId { get; set; }

    /// <summary>
    /// Навигационное свойство к договору
    /// </summary>
    public virtual Contract? Contract { get; set; }

    /// <summary>
    /// Номер акта
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// Дата составления
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Город составления
    /// </summary>
    public string City { get; set; } = string.Empty;
}
