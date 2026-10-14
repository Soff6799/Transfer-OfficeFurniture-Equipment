using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Entities;

namespace OfficeProcurement.Repositories.Contracts
{
    /// <summary>
    /// Репозиторий работы с <see cref="Contract"/>
    /// </summary>
    public interface IContractRepository : IBaseWriteRepository<Contract>
    {
        /// <summary>
        /// Получает сущности договоров
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Возвращает колекцию сущности договоров</returns>
        Task<IReadOnlyCollection<Contract>> GetContractAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получает договор по идентификатору
        /// </summary>
        /// <param name="id">Идент. акта</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Сущность договора или <c>null</c>, если не найдена</returns>
        Task<Contract?> GetContractByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получает сущность договора по номеру 
        /// </summary>
        /// <param name="number">Номер договора</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Сущность договора или <c>null</c>, если не найдена</returns>
        Task<Contract?> GetContractByNumberAsync(string number, CancellationToken cancellationToken);
    }
}
