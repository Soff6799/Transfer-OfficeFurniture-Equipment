namespace OfficeProcurement.Dal.Contracts.Interfaces;

public interface IEntityAuditDeletedAt
{
    /// <summary>
    /// Когда удалён
    /// </summary>
    DateTimeOffset? DeletedAt { get; set; }
}
