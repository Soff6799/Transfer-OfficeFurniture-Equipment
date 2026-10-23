using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Entities;

namespace OfficeProcurement.Repositories.Contracts
{
    /// <summary>
    /// Репозиторий работы с <see cref="TransferAct"/>
    /// </summary>
    public interface ITransferActRepository : IBaseWriteRepository<TransferAct>
    {
        /// <summary>
        /// Получает список всех актов
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Возвращает колекцию сущности актов</returns>
        Task<IReadOnlyCollection<TransferAct>> GetTransferActAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получает акт по идентификатору
        /// </summary>
        /// <param name="id">Идент. акта</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Сущность акта или <c>null</c>, если не найдена</returns>
        Task<TransferAct?> GetTransferActByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получает сущность акта по номеру акта
        /// </summary>
        /// <param name="number">номер акта</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Сущность акта или <c>null</c>, если не найдена</returns>
        Task<TransferAct?> GetTransferActByNumberAsync(string number, CancellationToken cancellationToken);

        /// <summary>
        /// Получает акт передачи с договором по идентификатору
        /// </summary>
        Task<TransferAct?> GetTransferActWithContractByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
