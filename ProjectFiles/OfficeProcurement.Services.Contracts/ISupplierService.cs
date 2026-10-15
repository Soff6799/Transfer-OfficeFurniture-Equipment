using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Services.Contracts;

/// <summary>
/// Сервис для работы с поставщиками
/// </summary>
public interface ISupplierService
{
    /// <summary>
    /// Получает список всех поставщиков
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Коллекция моделей поставщиков</returns>
    Task<IReadOnlyCollection<SupplierModel>> GetSuppliers(CancellationToken cancellationToken);

    /// <summary>
    /// Получает поставщика по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор поставщика</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Модель поставщика</returns>
    Task<SupplierModel> GetSupplierById(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Создаёт нового поставщика
    /// </summary>
    /// <param name="supplierCreateModel">Данные создаваемого поставщика</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Созданная модель поставщика</returns>
    Task<SupplierModel> CreateSupplier(SupplierCreateModel supplierCreateModel, CancellationToken cancellationToken);

    /// <summary>
    /// Обновляет данные поставщика
    /// </summary>
    /// <param name="supplierModel">Обновлённые данные поставщика</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task UpdateSupplier(SupplierModel supplierModel, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет поставщика по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор поставщика</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task DeleteSupplier(Guid id, CancellationToken cancellationToken);
}
