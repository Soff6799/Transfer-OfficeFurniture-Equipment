namespace OfficeProcurement.Dal.Contracts.Interfaces;

/// <summary>
/// Интерфейс для сущностей, отслеживающих информацию об изменении
/// дату/время последнего обновления и автора изменений
/// </summary>
public interface IEntityAuditUpdate
{
    /// <summary>
    /// Когда изменён
    /// </summary>
    DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// Кем изменён
    /// </summary>
    string UpdatedBy { get; set; }
}
