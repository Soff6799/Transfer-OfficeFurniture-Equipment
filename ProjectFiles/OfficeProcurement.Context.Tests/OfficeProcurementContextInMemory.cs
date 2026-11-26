using Microsoft.EntityFrameworkCore;
using OfficeProcurement.Context;
using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Infrastructure.Common;
using OfficeProcurement.Infrastructure.Providers;

namespace OfficeProcurement.Context.Tests;

/// <summary>
/// Тестовый контекст для InMemory базы данных
/// </summary>
public class OfficeProcurementContextInMemory : OfficeProcurementContext, IDbWriterContext
{

    /// <summary>
    /// Инициализирует тестовый контекст
    /// </summary>
    public OfficeProcurementContextInMemory(
        IDateTimeProvider? dateTimeProvider = null,
        IIdentityProvider? identityProvider = null)
        : base(
            new DbContextOptionsBuilder<OfficeProcurementContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options,
            dateTimeProvider ?? new DateTimeProvider(),
            identityProvider ?? new TestIdentityProvider())
    {
    }

}
