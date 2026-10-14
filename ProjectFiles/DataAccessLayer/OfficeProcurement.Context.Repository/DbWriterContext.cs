
using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Infrastructure.Common;

namespace OfficeProcurement.Context.Repository
{
    /// <summary>
    /// Контекст записи данных: объединяет писателя, поставщик времени и поставщик идентичности
    /// </summary>
    /// <param name="writer">Писатель сущностей в базу данных</param>
    /// <param name="dateTimeProvider">Поставщик текущей даты и времени</param>
    /// <param name="identityProvider">Поставщик информации о текущем пользователе</param>
    public class DbWriterContext(IWriter writer, IDateTimeProvider dateTimeProvider,
        IIdentityProvider identityProvider) : IDbWriterContext
    {
        /// <summary>
        /// Писатель сущностей в бд
        /// </summary>
        public IWriter Writer => writer;

        /// <summary>
        /// Поставщих текущей даты и времени
        /// </summary>
        public IDateTimeProvider DateTimeProvider => dateTimeProvider;

        /// <summary>
        /// Поставщик информации о текущем пользователе
        /// </summary>
        public IIdentityProvider IdentityProvider => identityProvider;
    }
}
