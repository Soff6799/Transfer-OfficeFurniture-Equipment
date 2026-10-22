using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Services.Contracts.Interface
{
    /// <summary>
    /// Сервис для работы с клиентами
    /// </summary>
    public interface IClientService
    {
        /// <summary>
        /// Получает список всех клиентов
        /// </summary>
        Task<IReadOnlyCollection<ClientModel>> GetClients(CancellationToken cancellationToken);

        /// <summary>
        /// Получает клиента по идентификатору
        /// </summary>
        Task<ClientModel> GetClientById(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Создаёт нового клиента
        /// </summary>
        Task<ClientModel> CreateClient(ClientCreateModel clientCreateModel, CancellationToken cancellationToken);

        /// <summary>
        /// Обновляет данные клиента
        /// </summary>
        Task UpdateClient(ClientModel clientModel, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет клиента по идентификатору
        /// </summary>
        Task DeleteClient(Guid id, CancellationToken cancellationToken);
    }
}
