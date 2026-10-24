using FluentAssertions;
using OfficeProcurement.Entities;
using OfficeProcurement.Repositories;
using OfficeProcurement.Repositories.Contracts;
using OfficeProcurement.Services;
using OfficeProcurement.Services.Contracts.Exceptions;
using OfficeProcurement.Services.Contracts.Interface;
using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.ServiceTests;

/// <summary>
/// Тесты для <see cref="TransferActService"/>
/// </summary>
public class TransferActServiceTests : BaseServiceTests
{
    private readonly ITransferActService transferActService;
    private readonly ITransferActRepository transferActRepository;

    /// <summary>
    /// Инициализирует тесты сервиса актов передачи
    /// </summary>
    public TransferActServiceTests()
    {
        transferActRepository = new TransferActRepository(WriterContext, Context);
        transferActService = new TransferActService(transferActRepository, UnitOfWork, Mapper);
    }

    /// <summary>
    /// Возвращает пустую коллекцию актов
    /// </summary>
    [Fact]
    public async Task GetTransferActsShouldReturnEmptyCollection()
    {
        // Act
        var result = await transferActService.GetTransferActs(CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Возвращает список актов
    /// </summary>
    [Fact]
    public async Task GetTransferActsShouldReturnActs()
    {
        // Arrange
        var act1 = new TransferAct { Number = "А-001", Date = DateOnly.FromDateTime(DateTime.Now), City = "Москва" };
        var act2 = new TransferAct { Number = "А-002", Date = DateOnly.FromDateTime(DateTime.Now), City = "Москва" };
        AddRange(act1, act2);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await transferActService.GetTransferActs(CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
    }

    /// <summary>
    /// Возвращает акт по идентификатору
    /// </summary>
    [Fact]
    public async Task GetTransferActByIdShouldReturnValue()
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
        var result = await transferActService.GetTransferActById(entity.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(entity.Id);
        result.Number.Should().Be("А-100");
    }

    /// <summary>
    /// Если акта нет, выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task GetTransferActByIdShouldThrowException()
    {
        // Act & Assert
        var act = () => transferActService.GetTransferActById(Guid.NewGuid(), CancellationToken.None);
        await act.Should().ThrowAsync<EntityNotFoundException<TransferAct>>();
    }

    /// <summary>
    /// Создаёт новый акт передачи
    /// </summary>
    [Fact]
    public async Task CreateTransferActShouldSaveToDatabase()
    {
        // Arrange
        var model = new TransferActCreateModel
        {
            Number = "А-NEW",
            Date = DateOnly.FromDateTime(DateTime.Now),
            City = "Москва"
        };

        // Act
        var result = await transferActService.CreateTransferAct(model, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Number.Should().Be("А-NEW");
        result.City.Should().Be("Москва");

        var saved = await transferActRepository.GetTransferActByNumberAsync("А-NEW", CancellationToken.None);
        saved.Should().NotBeNull();
    }

    /// <summary>
    /// Если акт с таким номером уже существует, выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task CreateTransferActShouldThrowExceptionIfNumberExists()
    {
        // Arrange
        var existing = new TransferAct { Number = "А-100", Date = DateOnly.FromDateTime(DateTime.Now), City = "Москва" };
        Add(existing);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var model = new TransferActCreateModel
        {
            Number = "А-100",
            Date = DateOnly.FromDateTime(DateTime.Now),
            City = "Москва"
        };

        // Act & Assert
        var act = () => transferActService.CreateTransferAct(model, CancellationToken.None);
        await act.Should().ThrowAsync<OfficeProcurementInvalidOperationException>();
    }

    /// <summary>
    /// Обновляет данные акта
    /// </summary>
    [Fact]
    public async Task UpdateTransferActShouldChangeData()
    {
        // Arrange
        var entity = new TransferAct
        {
            Id = Guid.NewGuid(),
            Number = "А-OLD",
            Date = DateOnly.FromDateTime(DateTime.Now),
            City = "Москва"
        };
        Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var model = new TransferActModel
        {
            Id = entity.Id,
            Number = "А-UPDATED",
            Date = DateOnly.FromDateTime(DateTime.Now),
            City = "Санкт-Петербург"
        };

        // Act
        await transferActService.UpdateTransferAct(model, CancellationToken.None);

        // Assert
        var updated = await transferActRepository.GetTransferActByIdAsync(entity.Id, CancellationToken.None);
        updated.Should().NotBeNull();
        updated!.Number.Should().Be("А-UPDATED");
        updated.City.Should().Be("Санкт-Петербург");
    }

    /// <summary>
    /// Если акта нет, обновление выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task UpdateTransferActShouldThrowExceptionIfNotFound()
    {
        // Arrange
        var model = new TransferActModel
        {
            Id = Guid.NewGuid(),
            Number = "А-NOTEXIST",
            Date = DateOnly.FromDateTime(DateTime.Now),
            City = "Москва"
        };

        // Act & Assert
        var act = () => transferActService.UpdateTransferAct(model, CancellationToken.None);
        await act.Should().ThrowAsync<EntityNotFoundException<TransferAct>>();
    }

    /// <summary>
    /// Если номер уже занят другим актом, обновление выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task UpdateTransferActShouldThrowExceptionIfNumberExists()
    {
        // Arrange
        var entity1 = new TransferAct
        {
            Id = Guid.NewGuid(),
            Number = "А-001",
            Date = DateOnly.FromDateTime(DateTime.Now),
            City = "Москва"
        };
        var entity2 = new TransferAct
        {
            Id = Guid.NewGuid(),
            Number = "А-002",
            Date = DateOnly.FromDateTime(DateTime.Now),
            City = "Москва"
        };
        AddRange(entity1, entity2);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var model = new TransferActModel
        {
            Id = entity1.Id,
            Number = "А-002",
            Date = DateOnly.FromDateTime(DateTime.Now),
            City = "Москва"
        };

        // Act & Assert
        var act = () => transferActService.UpdateTransferAct(model, CancellationToken.None);
        await act.Should().ThrowAsync<OfficeProcurementInvalidOperationException>();
    }

    /// <summary>
    /// Удаляет акт
    /// </summary>
    [Fact]
    public async Task DeleteTransferActShouldMarkAsDeleted()
    {
        // Arrange
        var entity = new TransferAct
        {
            Id = Guid.NewGuid(),
            Number = "А-DEL",
            Date = DateOnly.FromDateTime(DateTime.Now),
            City = "Москва"
        };
        Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        await transferActService.DeleteTransferAct(entity.Id, CancellationToken.None);

        // Assert
        var deleted = await transferActRepository.GetTransferActByIdAsync(entity.Id, CancellationToken.None);
        deleted.Should().BeNull();
    }

    /// <summary>
    /// Если акта нет, удаление выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task DeleteTransferActShouldThrowExceptionIfNotFound()
    {
        // Act & Assert
        var act = () => transferActService.DeleteTransferAct(Guid.NewGuid(), CancellationToken.None);
        await act.Should().ThrowAsync<EntityNotFoundException<TransferAct>>();
    }
}
