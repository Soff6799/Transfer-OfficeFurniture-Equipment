using FluentAssertions;
using OfficeProcurement.Entities;
using OfficeProcurement.Repositories.Contracts;

namespace OfficeProcurement.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="TransferActRepository"/>
/// </summary>
public class TransferActRepositoryTests : BaseRepositoryTests
{
    private readonly ITransferActRepository repository;

    /// <summary>
    /// Инициализирует тесты репозитория актов передачи
    /// </summary>
    public TransferActRepositoryTests()
    {
        repository = new TransferActRepository(WriterContext, Context);
    }

    /// <summary>
    /// Возвращает пустую коллекцию актов
    /// </summary>
    [Fact]
    public async Task GetTransferActAsyncShouldReturnEmptyCollection()
    {
        // Act
        var result = await repository.GetTransferActAsync(CancellationToken.None);

        // Assert
        result.Should().BeEmpty().And.NotBeNull();
    }

    /// <summary>
    /// Возвращает акты отсортированные по номеру
    /// </summary>
    [Fact]
    public async Task GetTransferActAsyncShouldReturnSortedByNumber()
    {
        // Arrange
        var act1 = new TransferAct { Number = "А-003", Date = DateOnly.FromDateTime(DateTime.Now), City = "Москва" };
        var act2 = new TransferAct { Number = "А-001", Date = DateOnly.FromDateTime(DateTime.Now), City = "Москва" };
        var act3 = new TransferAct { Number = "А-002", Date = DateOnly.FromDateTime(DateTime.Now), City = "Москва" };

        AddRange(act1, act2, act3);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = (await repository.GetTransferActAsync(CancellationToken.None)).ToList();

        // Assert
        result.Should().HaveCount(3);
        result[0].Number.Should().Be("А-001");
        result[1].Number.Should().Be("А-002");
        result[2].Number.Should().Be("А-003");
    }

    /// <summary>
    /// Возвращает акт по идентификатору если он существует
    /// </summary>
    [Fact]
    public async Task GetTransferActByIdAsyncShouldReturnValue()
    {
        // Arrange
        var entity = new TransferAct
        {
            Id = Guid.NewGuid(),
            Number = "А-100",
            Date = DateOnly.FromDateTime(DateTime.Now),
            City = "Москва"
        };
        Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetTransferActByIdAsync(entity.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(entity.Id);
    }

    /// <summary>
    /// Если акта нет, возвращает null
    /// </summary>
    [Fact]
    public async Task GetTransferActByIdAsyncShouldReturnNull()
    {
        // Act
        var result = await repository.GetTransferActByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Если акт удалён, возвращает null
    /// </summary>
    [Fact]
    public async Task GetTransferActByIdAsyncShouldReturnNullForDeleted()
    {
        // Arrange
        var item = new TransferAct
        {
            Id = Guid.NewGuid(),
            Number = "А-002",
            Date = DateOnly.FromDateTime(DateTime.Now),
            City = "Москва",
            DeletedAt = DateTimeOffset.Now
        };
        Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetTransferActByIdAsync(item.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает акт по номеру
    /// </summary>
    [Fact]
    public async Task GetTransferActByNumberAsyncShouldReturnValue()
    {
        // Arrange
        var item = new TransferAct
        {
            Number = "А-200",
            Date = DateOnly.FromDateTime(DateTime.Now),
            City = "Москва"
        };
        Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetTransferActByNumberAsync("А-200", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Number.Should().Be("А-200");
    }

    /// <summary>
    /// Если акта с таким номером нет, возвращает null
    /// </summary>
    [Fact]
    public async Task GetTransferActByNumberAsyncShouldReturnNull()
    {
        // Act
        var result = await repository.GetTransferActByNumberAsync("А-999", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Если акт удалён, по номеру возвращает null
    /// </summary>
    [Fact]
    public async Task GetTransferActByNumberAsyncShouldReturnNullForDeleted()
    {
        // Arrange
        var item = new TransferAct
        {
            Number = "А-300",
            Date = DateOnly.FromDateTime(DateTime.Now),
            City = "Москва",
            DeletedAt = DateTimeOffset.Now
        };
        Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetTransferActByNumberAsync("А-300", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Если акта нет, возвращает null
    /// </summary>
    [Fact]
    public async Task GetTransferActWithContractByIdAsyncShouldReturnNull()
    {
        // Act
        var result = await repository.GetTransferActWithContractByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Добавление акта сохраняет его в базе
    /// </summary>
    [Fact]
    public async Task AddShouldSaveTransferActToDatabase()
    {
        // Arrange
        var act = new TransferAct
        {
            Number = "А-NEW",
            Date = DateOnly.FromDateTime(DateTime.Now),
            City = "Москва"
        };

        // Act
        repository.Add(act);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Assert
        var saved = await repository.GetTransferActByNumberAsync("А-NEW", CancellationToken.None);
        saved.Should().NotBeNull();
        saved!.Number.Should().Be("А-NEW");
    }

    /// <summary>
    /// Обновление акта изменяет его данные
    /// </summary>
    [Fact]
    public async Task UpdateShouldChangeTransferActData()
    {
        // Arrange
        var act = new TransferAct
        {
            Number = "А-OLD",
            Date = DateOnly.FromDateTime(DateTime.Now),
            City = "Москва"
        };
        Add(act);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        act.City = "Санкт-Петербург";
        repository.Update(act);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Assert
        var updated = await repository.GetTransferActByIdAsync(act.Id, CancellationToken.None);
        updated.Should().NotBeNull();
        updated!.City.Should().Be("Санкт-Петербург");
    }
}
