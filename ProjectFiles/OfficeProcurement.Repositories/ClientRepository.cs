using System.Reflection.PortableExecutable;
using Microsoft.EntityFrameworkCore;
using OfficeProcurement.Context.Repository;
using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Entities;
using OfficeProcurement.Repositories.Contracts;

namespace OfficeProcurement.Repositories
{
    /// <summary>
    /// Репозиторий работы с клиентами
    /// </summary>
    public class ClientRepository : BaseWriteRepository<Client>, IClientRepository
    {
        private readonly IReader reader;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="ClientRepository"/>
        /// </summary>
        /// <param name="writerContext">Контекст репозитория записи сущностей</param>
        /// <param name="reader">Читатель сущностей из контекста</param>
        public ClientRepository(IDbWriterContext writerContext, IReader reader)
            : base(writerContext)
        {
            this.reader = reader;
        }

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<Client>> GetClientAsync(CancellationToken cancellationToken)
            => reader.Read<Client>()
                .NotDeletedAt()
                .OrderBy(x => x.CompanyName)
                .ToReadOnlyCollectionAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<Client?> GetClientByIdAsync(Guid id, CancellationToken cancellationToken)
            => reader.Read<Client>()
                .NotDeletedAt()
                .ById(id)
                .FirstOrDefaultAsync(cancellationToken);

        /// <inheritdoc/>
        public Task<Client?> GetClientByInnAsync(string inn, CancellationToken cancellationToken)
            => reader.Read<Client>()
            .FirstOrDefaultAsync(x => x.Inn == inn, cancellationToken);
    }
}
