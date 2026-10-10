
using OfficeProcurement.Infrastructure.Common;

namespace OfficeProcurement.Infrastructure.Providers
{
    /// <summary>
    /// поставщик пекущеё даты и времени
    /// </summary>
    public class DateTimeProvider : IDateTimeProvider
    {
        DateTimeOffset IDateTimeProvider.UtcNow => DateTimeOffset.UtcNow;

        DateTime IDateTimeProvider.LocalNow => DateTime.Now;
    }
}
