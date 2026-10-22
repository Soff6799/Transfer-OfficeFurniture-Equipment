using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Services.Contracts.Interface;

/// <summary>
/// Сервис для работы с договорами
/// </summary>
public interface IContractService
{
    /// <summary>
    /// Получает список всех договоров
    /// </summary>
    Task<IReadOnlyCollection<ContractModel>> GetContracts(CancellationToken cancellationToken);

    /// <summary>
    /// Получает договор по идентификатору
    /// </summary>
    Task<ContractModel> GetContractById(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Создаёт новый договор
    /// </summary>
    Task<ContractModel> CreateContract(ContractCreateModel contractCreateModel, CancellationToken cancellationToken);

    /// <summary>
    /// Обновляет данные договора
    /// </summary>
    Task UpdateContract(ContractModel contractModel, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет договор по идентификатору
    /// </summary>
    Task DeleteContract(Guid id, CancellationToken cancellationToken);
}
