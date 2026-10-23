using Microsoft.EntityFrameworkCore;
using OfficeProcurement.Context;
using OfficeProcurement.Dal.Contracts.Repositories;

namespace OfficeProcurement.Repositories.Tests;

/// <summary>
/// Базовый класс для тестов репозиториев с InMemory базой данных
/// </summary>
public abstract class OfficeProcurementContextInMemory : IDisposable
{
    protected readonly OfficeProcurementContext Context;
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IDbWriterContext WriterContext;

    /// <summary>
    /// Инициализирует тестовый контекст
    /// </summary>
    protected OfficeProcurementContextInMemory()
    {
        var options = new DbContextOptionsBuilder<OfficeProcurementContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new OfficeProcurementContext(options);
        WriterContext = (IDbWriterContext)Context;
        UnitOfWork = (IUnitOfWork)Context;
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}
