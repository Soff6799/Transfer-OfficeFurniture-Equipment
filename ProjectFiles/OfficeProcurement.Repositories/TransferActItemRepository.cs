using Microsoft.EntityFrameworkCore;
using OfficeProcurement.Context.Repository;
using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Entities;
using OfficeProcurement.Repositories.Contracts;

namespace OfficeProcurement.Repositories
{
    /// <summary>
    /// Репозиторий работы с позициями акта передачи
    /// </summary>
    public class TransferActItemRepository : BaseWriteRepository<TransferActItem>, ITransferActItemRepository
    {
        private readonly IReader reader;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="TransferActItemRepository"/>
        /// </summary>
        /// <param name="writerContext">Контекст репозитория записи сущностей</param>
        /// <param name="reader">Читатель сущностей из контекста</param>
        public TransferActItemRepository(IDbWriterContext writerContext, IReader reader)
            : base(writerContext)
        {
            this.reader = reader;
        }

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<TransferActItem>> GetTransferActItemAsync(CancellationToken cancellationToken)
            => reader.Read<TransferActItem>()
                .NotDeletedAt()
                .OrderBy(x => x.Name)
                .ToReadOnlyCollectionAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<TransferActItem?> GetTransferActItemByIdAsync(Guid id, CancellationToken cancellationToken)
            => reader.Read<TransferActItem>()
                .NotDeletedAt()
                .ById(id)
                .FirstOrDefaultAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<TransferActItem?> GetTransferActItemByStockNumberAsync(string stockNumber, CancellationToken cancellationToken)
            => reader.Read<TransferActItem>()
                .FirstOrDefaultAsync(x => x.StockNumber == stockNumber, cancellationToken);
    }
}
