namespace OfficeProcurement.Dal.Contracts.Interfaces;

/// <summary>
/// Интерфейс для сущностей, поддерживающих мягкое удаление soft delete
/// хранит дату и время удаления сущности
/// </summary>
public interface IEntityAuditDeletedAt
{
    /// <summary>
    /// Когда удалён
    /// </summary>
    DateTimeOffset? DeletedAt { get; set; }
}
