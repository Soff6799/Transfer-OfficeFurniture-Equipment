namespace OfficeProcurement.Dal.Contracts;

public interface IEntityWithId
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    Guid Id { get; set; }
}
