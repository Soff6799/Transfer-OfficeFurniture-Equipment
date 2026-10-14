
namespace OfficeProcurement.Infrastructure.Common
{
    /// <summary>
    /// Интерфейс получения даты
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// текущий момент utc
        /// </summary>
        DateTimeOffset UtcNow { get; }

        /// <summary>
        /// текущий момент (локальное время)
        /// </summary>
        DateTime LocalNow { get; }

    }
}
