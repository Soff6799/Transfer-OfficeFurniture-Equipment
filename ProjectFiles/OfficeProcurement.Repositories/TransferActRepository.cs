using Microsoft.EntityFrameworkCore;
using OfficeProcurement.Context.Repository;
using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Entities;
using OfficeProcurement.Repositories.Contracts;

namespace OfficeProcurement.Repositories
{
    /// <summary>
    /// Репозиторий работы с актами передачи
    /// </summary>
    public class TransferActRepository : BaseWriteRepository<TransferAct>, ITransferActRepository
    {
        private readonly IReader reader;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="TransferActRepository"/>
        /// </summary>
        /// <param name="writerContext">Контекст репозитория записи сущностей</param>
        /// <param name="reader">Читатель сущностей из контекста</param>
        public TransferActRepository(IDbWriterContext writerContext, IReader reader)
            : base(writerContext)
        {
            this.reader = reader;
        }

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<TransferAct>> GetTransferActAsync(CancellationToken cancellationToken)
            => reader.Read<TransferAct>()
                .NotDeletedAt()
                .OrderBy(x => x.Number)
                .ToReadOnlyCollectionAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<TransferAct?> GetTransferActByIdAsync(Guid id, CancellationToken cancellationToken)
            => reader.Read<TransferAct>()
                .NotDeletedAt()
                .ById(id)
                .FirstOrDefaultAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<TransferAct?> GetTransferActByNumberAsync(string number, CancellationToken cancellationToken)
            => reader.Read<TransferAct>()
                .NotDeletedAt()
                .Where(x => x.Number == number)
                .FirstOrDefaultAsync(cancellationToken);
    }
}
