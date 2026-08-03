namespace OfficeProcurement.Dal.Contracts;

public interface IEntityAuditDeletedAt
{
    /// <summary>
    /// Когда удалён
    /// </summary>
    DateTimeOffset? DeletedAt { get; set; }
}
