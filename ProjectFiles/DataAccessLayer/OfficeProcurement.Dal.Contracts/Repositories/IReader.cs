using OfficeProcurement.Dal.Contracts.Interfaces;
namespace OfficeProcurement.Dal.Contracts.Repositories;

/// <summary>
/// Интерфейс для чтения сущностей из базы данных
/// </summary>
public interface IReader
{
    IQueryable<TEntity> Read<TEntity>() where TEntity : class, IEntity;
}




