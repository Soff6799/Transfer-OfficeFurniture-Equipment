using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Services.Contracts.Interface;

/// <summary>
/// Сервис для работы с продуктами
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Получает список всех продуктов
    /// </summary>
    Task<IReadOnlyCollection<ProductModel>> GetProducts(CancellationToken cancellationToken);

    /// <summary>
    /// Получает продукт по идентификатору
    /// </summary>
    Task<ProductModel> GetProductById(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Создаёт новый продукт
    /// </summary>
    Task<ProductModel> CreateProduct(ProductCreateModel productCreateModel, CancellationToken cancellationToken);

    /// <summary>
    /// Обновляет данные продукта
    /// </summary>
    Task UpdateProduct(ProductModel productModel, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет продукт по идентификатору
    /// </summary>
    Task DeleteProduct(Guid id, CancellationToken cancellationToken);
}
