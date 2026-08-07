using OfficeProcurement.Dal.Contracts.Interfaces;

namespace OfficeProcurement.Dal.Contracts;

public abstract class BaseAuditEntity : IEntityWithId, IEntityAuditCreated, IEntityAuditUpdate, IEntityAuditDeletedAt
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Когда создан
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Кем создан
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Когда изменён
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// Кем изменён
    /// </summary>
    public string UpdatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Когда удалён
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }
}
