using Microsoft.EntityFrameworkCore;
using OfficeProcurement.Context.Repository;
using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Entities;
using OfficeProcurement.Repositories.Contracts;

namespace OfficeProcurement.Repositories
{
    /// <summary>
    /// Репозиторий работы с продуктами
    /// </summary>
    public class ProductRepository : BaseWriteRepository<Product>, IProductRepository
    {
        private readonly IReader reader;

        /// <summary>
        /// Инициализирует новый экземпляр<see cref="ProductRepository"/>
        /// </summary>
        /// <param name="writerContext">Контекст репозитория записи сущностей</param>
        /// <param name="reader">Читатель сущностей из контекста</param>
        public ProductRepository(IDbWriterContext writerContext, IReader reader)
            : base(writerContext)
        {
            this.reader = reader;
        }

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<Product>> GetProductAsync(CancellationToken cancellationToken)
            => reader.Read<Product>()
                .NotDeletedAt()
                .OrderBy(x => x.Name)
                .ToReadOnlyCollectionAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
            => reader.Read<Product>()
                .NotDeletedAt()
                .ById(id)
                .FirstOrDefaultAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<Product?> GetProductByStockNumberAsync(string stockNumber, CancellationToken cancellationToken)
            => reader.Read<Product>()
                .NotDeletedAt()
                .FirstOrDefaultAsync(x => x.StockNumber == stockNumber, cancellationToken);
    }
}
