using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Entities;

namespace OfficeProcurement.Repositories.Contracts
{
    /// <summary>
    /// Репозиторий работы с <see cref="Client"/>
    /// </summary>
    public interface IClientRepository : IBaseWriteRepository<Client>
    {
        /// <summary>
        /// Получает сущности всех клиентов
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Возвращает колекцию сущности клиентов</returns>
        Task<IReadOnlyCollection<Client>> GetClientAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получает сущность клиента по идентификатору
        /// </summary>
        /// <param name="id">Идент. клиента</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Сущность клиента или <c>null</c>, если не найдена</returns>
        Task<Client?> GetClientByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получает сущность клиента по ИНН
        /// </summary>
        Task<Client?> GetClientByInnAsync(string inn, CancellationToken cancellationToken);
    }
}
