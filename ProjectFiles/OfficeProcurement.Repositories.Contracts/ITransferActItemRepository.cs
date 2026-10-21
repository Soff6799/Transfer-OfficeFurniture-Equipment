using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Entities;

namespace OfficeProcurement.Repositories.Contracts
{
    /// <summary>
    /// Репозиторий работы с <see cref="TransferActItem"/>
    /// </summary>
    public interface ITransferActItemRepository : IBaseWriteRepository<TransferActItem>
    {
        /// <summary>
        /// Получает список всех строк содержимого акта
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Возвращает колекцию сущности содержаний актов</returns>
        Task<IReadOnlyCollection<TransferActItem>> GetTransferActItemAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получает строку содержимого акта по идентификатору
        /// </summary>
        /// <param name="id">Идент. содержимого акта</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Сущность продукта или <c>null</c>, если не найдена</returns>
        Task<TransferActItem?> GetTransferActItemByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получает сущность позиции акта по складскому номеру
        /// </summary>
        /// <param name="stockNumber">Складской номер позиции</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Сущность содержимого акта или <c>null</c>, если не найдена</returns>
        Task<TransferActItem?> GetTransferActItemByStockNumberAsync(string stockNumber, CancellationToken cancellationToken);
    }
}
