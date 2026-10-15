using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Services.Contracts.Interface;

/// <summary>
/// Сервис для работы с актами передачи
/// </summary>
public interface ITransferActService
{
    /// <summary>
    /// Получает список всех актов передачи
    /// </summary>
    Task<IReadOnlyCollection<TransferActModel>> GetTransferActs(CancellationToken cancellationToken);

    /// <summary>
    /// Получает акт передачи по идентификатору
    /// </summary>
    Task<TransferActModel> GetTransferActById(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Создаёт новый акт передачи
    /// </summary>
    Task<TransferActModel> CreateTransferAct(TransferActCreateModel transferActCreateModel, CancellationToken cancellationToken);

    /// <summary>
    /// Обновляет данные акта передачи
    /// </summary>
    Task UpdateTransferAct(TransferActModel transferActModel, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет акт передачи по идентификатору
    /// </summary>
    Task DeleteTransferAct(Guid id, CancellationToken cancellationToken);
}
