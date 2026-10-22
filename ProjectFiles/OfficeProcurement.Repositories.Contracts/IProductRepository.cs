using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Entities;

namespace OfficeProcurement.Repositories.Contracts
{
    /// <summary>
    /// Репозиторий работы с <see cref="Product"/>
    /// </summary>
    public interface IProductRepository : IBaseWriteRepository<Product>
    {
        /// <summary>
        /// Получает список всех продуктов
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Возвращает колекцию сущности продуктов</returns>
        Task<IReadOnlyCollection<Product>> GetProductAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получает продукт по идентификатору
        /// </summary>
        /// <param name="id">Идент. продукта</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Сущность продукта или <c>null</c>, если не найдена</returns>
        Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получает продукт по складскому номеру
        /// </summary>
        /// <param name="stockNumber">Складской номер продукта</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Сущность продукта или <c>null</c>, если не найдена</returns>
        Task<Product?> GetProductByStockNumberAsync(string stockNumber, CancellationToken cancellationToken);
    }
}
