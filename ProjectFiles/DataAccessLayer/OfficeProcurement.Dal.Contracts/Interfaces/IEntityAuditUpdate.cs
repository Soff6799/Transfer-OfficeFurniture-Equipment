namespace OfficeProcurement.Dal.Contracts.Interfaces;

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
