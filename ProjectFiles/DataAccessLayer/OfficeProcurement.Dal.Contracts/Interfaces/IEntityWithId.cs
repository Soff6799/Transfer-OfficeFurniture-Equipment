namespace OfficeProcurement.Dal.Contracts.Interfaces;

/// <summary>
/// Интерфейс для сущностей, имеющих первичный ключ (идентификатор)
/// </summary>
public interface IEntityWithId
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    Guid Id { get; set; }
}
