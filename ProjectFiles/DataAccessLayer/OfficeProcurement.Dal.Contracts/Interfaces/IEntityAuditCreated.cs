namespace OfficeProcurement.Dal.Contracts;

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
