namespace OfficeProcurement.Dal.Contracts.Interfaces;

/// <summary>
/// Интерфейс для сущностей, отслеживающих информацию о создании
/// дату/время создания и автора
/// </summary>
public interface IEntityAuditCreated
{
    /// <summary>
    /// Когда создан
    /// </summary>
    DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Кем создан
    /// </summary>
    string CreatedBy { get; set; }
}
