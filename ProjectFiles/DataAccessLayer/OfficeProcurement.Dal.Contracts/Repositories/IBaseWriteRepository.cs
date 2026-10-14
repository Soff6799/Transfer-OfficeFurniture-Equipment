using OfficeProcurement.Dal.Contracts.Interfaces;

namespace OfficeProcurement.Dal.Contracts.Repositories
{
    /// <summary>
    /// Базовый интерфейс репозитория записи сущностей в бд
    /// </summary>
    public interface IBaseWriteRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// Добавляет новую сущность
        /// </summary>
        /// <param name="entity">добавляемая сущность</param>
        void Add(T entity);

        /// <summary>
        /// Удаляет сущность
        /// </summary>
        /// <param name="entity">удаляемая сущность</param>
        void Delete(T entity);

        /// <summary>
        /// Обновляет сущность
        /// </summary>
        /// <param name="entity">обновляемая сущность</param>
        void Update(T entity);

    }
}
