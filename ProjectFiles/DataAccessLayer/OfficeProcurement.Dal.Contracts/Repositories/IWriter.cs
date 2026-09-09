using OfficeProcurement.Dal.Contracts.Interfaces;
namespace OfficeProcurement.Dal.Contracts.Repositories;

/// <summary>
/// Интерфейс создания и модификации записей в контексте
/// </summary>
public interface IWriter
{
    /// <summary>
    /// Добавить новую запись
    /// </summary>
    void Add<TEntity>(TEntity entity) where TEntity : class, IEntity;

    /// <summary>
    /// Обновить/Изменить запись
    /// </summary>
    void Update<TEntity>(TEntity entity) where TEntity : class, IEntity;

    /// <summary>
    /// Удалить запись
    /// </summary>
    void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity;
}
