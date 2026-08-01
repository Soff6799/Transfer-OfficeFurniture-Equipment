using OfficeProcurement.Dal.Contracts;

namespace OfficeProcurement.Entities;

/// <summary>
/// Сущность самого акта
/// </summary>
public class TransferAct: BaseAuditEntity
{
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
