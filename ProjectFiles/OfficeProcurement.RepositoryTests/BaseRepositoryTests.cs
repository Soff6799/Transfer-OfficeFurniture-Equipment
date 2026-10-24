using OfficeProcurement.Context.Tests;
using OfficeProcurement.Dal.Contracts.Repositories;

namespace OfficeProcurement.Repositories.Tests;

/// <summary>
/// Базовый класс для тестов репозиториев с InMemory базой данных
/// </summary>
public abstract class BaseRepositoryTests : IDisposable
{
    protected readonly OfficeProcurementContextInMemory Context;
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IDbWriterContext WriterContext;
    protected readonly IReader Reader;

    /// <summary>
    /// Инициализирует тестовый контекст
    /// </summary>
    protected BaseRepositoryTests()
    {
        Context = new OfficeProcurementContextInMemory();
        WriterContext = Context;
        UnitOfWork = Context;
        Reader = Context;
    }

    /// <summary>
    /// Добавляет сущность в контекст
    /// </summary>
    protected void Add<TEntity>(TEntity entity) where TEntity : class
    {
        Context.Set<TEntity>().Add(entity);
    }

    /// <summary>
    /// Добавляет несколько сущностей в контекст
    /// </summary>
    protected void AddRange<TEntity>(params TEntity[] entities) where TEntity : class
    {
        Context.Set<TEntity>().AddRange(entities);
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}
