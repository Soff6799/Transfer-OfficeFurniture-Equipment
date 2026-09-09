namespace OfficeProcurement.Dal.Contracts.Interfaces;

public interface IEntityWithId
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    Guid Id { get; set; }
}
