using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Services.Contracts.Interface;

/// <summary>
/// Сервис для работы с позициями акта передачи
/// </summary>
public interface ITransferActItemService
{
    /// <summary>
    /// Получает список всех позиций акта передачи
    /// </summary>
    Task<IReadOnlyCollection<TransferActItemModel>> GetTransferActItems(CancellationToken cancellationToken);

    /// <summary>
    /// Получает позицию акта по идентификатору
    /// </summary>
    Task<TransferActItemModel> GetTransferActItemById(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Создаёт новую позицию акта передачи
    /// </summary>
    Task<TransferActItemModel> CreateTransferActItem(TransferActItemCreateModel transferActItemCreateModel, CancellationToken cancellationToken);

    /// <summary>
    /// Обновляет данные позиции акта
    /// </summary>
    Task UpdateTransferActItem(TransferActItemModel transferActItemModel, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет позицию акта по идентификатору
    /// </summary>
    Task DeleteTransferActItem(Guid id, CancellationToken cancellationToken);
}
