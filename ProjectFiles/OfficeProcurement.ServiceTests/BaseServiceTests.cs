using AutoMapper;
using OfficeProcurement.Context.Tests;
using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Services.Automapper;

namespace OfficeProcurement.ServiceTests;

/// <summary>
/// Базовый класс для тестов сервисов
/// </summary>
public abstract class BaseServiceTests : IDisposable
{
    protected readonly OfficeProcurementContextInMemory Context;
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IDbWriterContext WriterContext;
    protected readonly IMapper Mapper;

    /// <summary>
    /// Инициализирует тестовый контекст и маппер
    /// </summary>
    protected BaseServiceTests()
    {
        Context = new OfficeProcurementContextInMemory();
        WriterContext = Context;
        UnitOfWork = Context;
        var profile = new ServiceProfile();
        Mapper = new MapperConfiguration(x => x.AddProfile(profile)).CreateMapper();
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
