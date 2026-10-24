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
/// Тесты для <see cref="TransferActItemService"/>
/// </summary>
public class TransferActItemServiceTests : BaseServiceTests
{
    private readonly ITransferActItemService transferActItemService;
    private readonly ITransferActItemRepository transferActItemRepository;

    /// <summary>
    /// Инициализирует тесты сервиса позиций акта
    /// </summary>
    public TransferActItemServiceTests()
    {
        transferActItemRepository = new TransferActItemRepository(WriterContext, Context);
        transferActItemService = new TransferActItemService(transferActItemRepository, UnitOfWork, Mapper);
    }

    /// <summary>
    /// Возвращает пустую коллекцию позиций
    /// </summary>
    [Fact]
    public async Task GetTransferActItemsShouldReturnEmptyCollection()
    {
        // Act
        var result = await transferActItemService.GetTransferActItems(CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Возвращает список позиций
    /// </summary>
    [Fact]
    public async Task GetTransferActItemsShouldReturnItems()
    {
        // Arrange
        var item1 = new TransferActItem { Name = "Стол", Unit = "шт" };
        var item2 = new TransferActItem { Name = "Стул", Unit = "шт" };
        AddRange(item1, item2);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await transferActItemService.GetTransferActItems(CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
    }

    /// <summary>
    /// Возвращает позицию по идентификатору
    /// </summary>
    [Fact]
    public async Task GetTransferActItemByIdShouldReturnValue()
    {
        // Arrange
        var entity = new TransferActItem
        {
            Id = Guid.NewGuid(),
            Name = "Стол офисный",
            StockNumber = "ST-001",
            Unit = "шт",
            Quantity = 5,
            UnitPrice = 15000
        };
        Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await transferActItemService.GetTransferActItemById(entity.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(entity.Id);
        result.Name.Should().Be("Стол офисный");
    }

    /// <summary>
    /// Если позиции нет, выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task GetTransferActItemByIdShouldThrowException()
    {
        // Act & Assert
        var act = () => transferActItemService.GetTransferActItemById(Guid.NewGuid(), CancellationToken.None);
        await act.Should().ThrowAsync<EntityNotFoundException<TransferActItem>>();
    }

    /// <summary>
    /// Создаёт новую позицию акта
    /// </summary>
    [Fact]
    public async Task CreateTransferActItemShouldSaveToDatabase()
    {
        // Arrange
        var model = new TransferActItemCreateModel
        {
            Name = "Новая позиция",
            StockNumber = "ST-NEW",
            Unit = "шт",
            Quantity = 10,
            UnitPrice = 5000
        };

        // Act
        var result = await transferActItemService.CreateTransferActItem(model, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Новая позиция");
        result.StockNumber.Should().Be("ST-NEW");
        var saved = await transferActItemRepository.GetTransferActItemByStockNumberAsync("ST-NEW", CancellationToken.None);
        saved.Should().NotBeNull();
    }

    /// <summary>
    /// Создаёт позицию без складского номера
    /// </summary>
    [Fact]
    public async Task CreateTransferActItemWithoutStockNumberShouldSucceed()
    {
        // Arrange
        var model = new TransferActItemCreateModel
        {
            Name = "Позиция без номера",
            StockNumber = null,
            Unit = "шт",
            Quantity = 1,
            UnitPrice = 1000
        };

        // Act
        var result = await transferActItemService.CreateTransferActItem(model, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Позиция без номера");
    }

    /// <summary>
    /// Если позиция с таким складским номером уже существует, выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task CreateTransferActItemShouldThrowExceptionIfStockNumberExists()
    {
        // Arrange
        var existing = new TransferActItem { Name = "Старая", StockNumber = "ST-100", Unit = "шт" };
        Add(existing);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var model = new TransferActItemCreateModel
        {
            Name = "Новая",
            StockNumber = "ST-100",
            Unit = "шт",
            Quantity = 1,
            UnitPrice = 2000
        };

        // Act & Assert
        var act = () => transferActItemService.CreateTransferActItem(model, CancellationToken.None);
        await act.Should().ThrowAsync<OfficeProcurementInvalidOperationException>();
    }

    /// <summary>
    /// Обновляет данные позиции
    /// </summary>
    [Fact]
    public async Task UpdateTransferActItemShouldChangeData()
    {
        // Arrange
        var entity = new TransferActItem
        {
            Id = Guid.NewGuid(),
            Name = "Старое название",
            StockNumber = "ST-OLD",
            Unit = "шт",
            Quantity = 1,
            UnitPrice = 1000
        };
        Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var model = new TransferActItemModel
        {
            Id = entity.Id,
            Name = "Новое название",
            StockNumber = "ST-OLD",
            Unit = "шт",
            Quantity = 5,
            UnitPrice = 2000
        };

        // Act
        await transferActItemService.UpdateTransferActItem(model, CancellationToken.None);

        // Assert
        var updated = await transferActItemRepository.GetTransferActItemByIdAsync(entity.Id, CancellationToken.None);
        updated.Should().NotBeNull();
        updated!.Name.Should().Be("Новое название");
        updated.Quantity.Should().Be(5);
        updated.UnitPrice.Should().Be(2000);
    }

    /// <summary>
    /// Если позиции нет, обновление выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task UpdateTransferActItemShouldThrowExceptionIfNotFound()
    {
        // Arrange
        var model = new TransferActItemModel
        {
            Id = Guid.NewGuid(),
            Name = "Несуществующая",
            StockNumber = "ST-NO",
            Unit = "шт",
            Quantity = 1,
            UnitPrice = 1000
        };

        // Act & Assert
        var act = () => transferActItemService.UpdateTransferActItem(model, CancellationToken.None);
        await act.Should().ThrowAsync<EntityNotFoundException<TransferActItem>>();
    }

    /// <summary>
    /// Если складской номер уже занят другой позицией, обновление выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task UpdateTransferActItemShouldThrowExceptionIfStockNumberExists()
    {
        // Arrange
        var entity1 = new TransferActItem
        {
            Id = Guid.NewGuid(),
            Name = "Позиция 1",
            StockNumber = "ST-001",
            Unit = "шт"
        };
        var entity2 = new TransferActItem
        {
            Id = Guid.NewGuid(),
            Name = "Позиция 2",
            StockNumber = "ST-002",
            Unit = "шт"
        };
        AddRange(entity1, entity2);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var model = new TransferActItemModel
        {
            Id = entity1.Id,
            Name = "Позиция 1 обновлённая",
            StockNumber = "ST-002",
            Unit = "шт",
            Quantity = 1,
            UnitPrice = 1000
        };

        // Act & Assert
        var act = () => transferActItemService.UpdateTransferActItem(model, CancellationToken.None);
        await act.Should().ThrowAsync<OfficeProcurementInvalidOperationException>();
    }

    /// <summary>
    /// Удаляет позицию
    /// </summary>
    [Fact]
    public async Task DeleteTransferActItemShouldMarkAsDeleted()
    {
        // Arrange
        var entity = new TransferActItem
        {
            Id = Guid.NewGuid(),
            Name = "Удаляемая",
            StockNumber = "ST-DEL",
            Unit = "шт"
        };
        Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        await transferActItemService.DeleteTransferActItem(entity.Id, CancellationToken.None);

        // Assert
        var deleted = await transferActItemRepository.GetTransferActItemByIdAsync(entity.Id, CancellationToken.None);
        deleted.Should().BeNull();
    }

    /// <summary>
    /// Если позиции нет, удаление выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task DeleteTransferActItemShouldThrowExceptionIfNotFound()
    {
        // Act & Assert
        var act = () => transferActItemService.DeleteTransferActItem(Guid.NewGuid(), CancellationToken.None);
        await act.Should().ThrowAsync<EntityNotFoundException<TransferActItem>>();
    }
}
