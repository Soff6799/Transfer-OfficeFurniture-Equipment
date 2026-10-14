using Microsoft.EntityFrameworkCore;
using OfficeProcurement.Context.Repository;
using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Entities;
using OfficeProcurement.Repositories.Contracts;

namespace OfficeProcurement.Repositories
{
    /// <summary>
    /// Репозиторий работы с договорами
    /// </summary>
    public class ContractRepository : BaseWriteRepository<Contract>, IContractRepository
    {
        private readonly IReader reader;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="ContractRepository"/>.
        /// </summary>
        /// <param name="writerContext">Контекст репозитория записи сущностей</param>
        /// <param name="reader">Читатель сущностей из контекста</param>
        public ContractRepository(IDbWriterContext writerContext, IReader reader)
            : base(writerContext)
        {
            this.reader = reader;
        }

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<Contract>> GetContractAsync(CancellationToken cancellationToken)
            => reader.Read<Contract>()
                .NotDeletedAt()
                .OrderBy(x => x.Number)
                .ToReadOnlyCollectionAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<Contract?> GetContractByIdAsync(Guid id, CancellationToken cancellationToken)
            => reader.Read<Contract>()
                .NotDeletedAt()
                .ById(id)
                .FirstOrDefaultAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<Contract?> GetContractByNumberAsync(string number, CancellationToken cancellationToken)
            => reader.Read<Contract>()
                .NotDeletedAt()
                .Where(x => x.Number == number)
                .FirstOrDefaultAsync(cancellationToken);
    }
}
