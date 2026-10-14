using Microsoft.EntityFrameworkCore;
using OfficeProcurement.Context.Repository;
using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Entities;
using OfficeProcurement.Repositories.Contracts;

namespace OfficeProcurement.Repositories
{
    /// <summary>
    /// Репозиторий работы с поставщиками
    /// </summary>
    public class SupplierRepository : BaseWriteRepository<Supplier>, ISupplierRepository
    {
        private readonly IReader reader;

        /// <summary>
        /// Инициализирует новый экземпляр<see cref="SupplierRepository"/>
        /// </summary>
        /// <param name="writerContext">Контекст репозитория записи сущностей</param>
        /// <param name="reader">Читатель сущностей из контекста</param>
        public SupplierRepository(IDbWriterContext writerContext, IReader reader)
            : base(writerContext)
        {
            this.reader = reader;
        }

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<Supplier>> GetSupplierAsync(CancellationToken cancellationToken)
            => reader.Read<Supplier>()
                .NotDeletedAt()
                .OrderBy(x => x.CompanyName)
                .ToReadOnlyCollectionAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<Supplier?> GetSupplierByIdAsync(Guid id, CancellationToken cancellationToken)
            => reader.Read<Supplier>()
                .NotDeletedAt()
                .ById(id)
                .FirstOrDefaultAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<Supplier?> GetSupplierByCompanyNameAsync(string companyName, CancellationToken cancellationToken)
            => reader.Read<Supplier>()
                .NotDeletedAt()
                .Where(x => x.CompanyName.Contains(companyName))
                .FirstOrDefaultAsync(cancellationToken);
    }
}
