using OfficeProcurement.Infrastructure.Common;
namespace OfficeProcurement.Dal.Contracts.Repositories
{
    /// <summary>
    /// Определяет контекст репозитория записи сущности
    /// (объединяет в себе всё то что сопутствуется при записи)
    /// </summary>
    public interface IDbWriterContext
    {
        /// <inheritdoc cref="IWriter"/>
        IWriter Writer { get; }

        /// <inheritdoc cref="IDateTimeProvider"/>
        IDateTimeProvider DateTimeProvider { get; }

        /// <inheritdoc cref="IIdentityProvider"/>
        IIdentityProvider IdentityProvider { get; }
    }
}
