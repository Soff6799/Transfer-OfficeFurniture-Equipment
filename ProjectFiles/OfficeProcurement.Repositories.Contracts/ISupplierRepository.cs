using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Entities;

namespace OfficeProcurement.Repositories.Contracts
{
    /// <summary>
    /// Репозиторий работы с <see cref="Supplier"/>
    /// </summary>
    public interface ISupplierRepository : IBaseWriteRepository<Supplier>
    {
        /// <summary>
        /// Получает сущности поставщиков
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Возвращает колекцию сущности поставщика</returns>
        Task<IReadOnlyCollection<Supplier>> GetSupplierAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получает поставщика по идентификатору
        /// </summary>
        /// <param name="id">Идент. поставщика</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Сущность поставщика или <c>null</c>, если не найдена</returns>
        Task<Supplier?> GetSupplierByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получает сущность поставщика по ИНН
        /// </summary>
        Task<Supplier?> GetSupplierByInnAsync(string inn, CancellationToken cancellationToken);
    }
}
