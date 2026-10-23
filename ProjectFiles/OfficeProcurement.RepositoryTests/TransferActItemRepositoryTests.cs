using FluentAssertions;
using OfficeProcurement.Entities;
using OfficeProcurement.Repositories.Contracts;
using Xunit;

namespace OfficeProcurement.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="TransferActItemRepository"/>
/// </summary>
public class TransferActItemRepositoryTests : OfficeProcurementContextInMemory
{
    private readonly ITransferActItemRepository repository;

    /// <summary>
    /// Инициализирует тесты репозитория позиций актов
    /// </summary>
    public TransferActItemRepositoryTests()
    {
        repository = new TransferActItemRepository(WriterContext, Context);
    }

    /// <summary>
    /// Возвращает пустую коллекцию позиций
    /// </summary>
    [Fact]
    public async Task GetTransferActItemAsyncShouldReturnEmptyCollection()
    {
        // Act
        var result = await repository.GetTransferActItemAsync(CancellationToken.None);

        // Assert
        result.Should().BeEmpty().And.NotBeNull();
    }

    /// <summary>
    /// Возвращает позиции отсортированные по названию
    /// </summary>
    [Fact]
    public async Task GetTransferActItemAsyncShouldReturnSortedByName()
    {
        // Arrange
        var item1 = new TransferActItem { Name = "Стул", Unit = "шт" };
        var item2 = new TransferActItem { Name = "Стол", Unit = "шт" };
        var item3 = new TransferActItem { Name = "Шкаф", Unit = "шт" };

        Context.AddRange(item1, item2, item3);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = (await repository.GetTransferActItemAsync(CancellationToken.None)).ToList();

        // Assert
        result.Should().HaveCount(3);
        result[0].Name.Should().Be("Стол");
        result[1].Name.Should().Be("Стул");
        result[2].Name.Should().Be("Шкаф");
    }

    /// <summary>
    /// Возвращает позицию по идентификатору если она существует
    /// </summary>
    [Fact]
    public async Task GetTransferActItemByIdAsyncShouldReturnValue()
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
        Context.Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetTransferActItemByIdAsync(entity.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(entity.Id);
    }

    /// <summary>
    /// Если позиции нет, возвращает null
    /// </summary>
    [Fact]
    public async Task GetTransferActItemByIdAsyncShouldReturnNull()
    {
        // Act
        var result = await repository.GetTransferActItemByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Если позиция удалена, возвращает null
    /// </summary>
    [Fact]
    public async Task GetTransferActItemByIdAsyncShouldReturnNullForDeleted()
    {
        // Arrange
        var item = new TransferActItem
        {
            Id = Guid.NewGuid(),
            Name = "Удалённая",
            Unit = "шт",
            DeletedAt = DateTimeOffset.Now
        };
        Context.Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetTransferActItemByIdAsync(item.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает позицию по складскому номеру
    /// </summary>
    [Fact]
    public async Task GetTransferActItemByStockNumberAsyncShouldReturnValue()
    {
        // Arrange
        var item = new TransferActItem
        {
            Name = "Стол",
            StockNumber = "ST-100",
            Unit = "шт"
        };
        Context.Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetTransferActItemByStockNumberAsync("ST-100", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.StockNumber.Should().Be("ST-100");
    }

    /// <summary>
    /// Если позиции с таким складским номером нет, возвращает null
    /// </summary>
    [Fact]
    public async Task GetTransferActItemByStockNumberAsyncShouldReturnNull()
    {
        // Act
        var result = await repository.GetTransferActItemByStockNumberAsync("ST-999", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Если позиция удалена, по складскому номеру возвращает null
    /// </summary>
    [Fact]
    public async Task GetTransferActItemByStockNumberAsyncShouldReturnNullForDeleted()
    {
        // Arrange
        var item = new TransferActItem
        {
            Name = "Удалённая",
            StockNumber = "ST-DEL",
            Unit = "шт",
            DeletedAt = DateTimeOffset.Now
        };
        Context.Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetTransferActItemByStockNumberAsync("ST-DEL", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает позиции по идентификатору акта
    /// </summary>
    [Fact]
    public async Task GetTransferActItemsByTransferActIdAsyncShouldReturnValue()
    {
        // Arrange
        var actId = Guid.NewGuid();
        var item1 = new TransferActItem { Name = "Стол", Unit = "шт", TransferActId = actId };
        var item2 = new TransferActItem { Name = "Стул", Unit = "шт", TransferActId = actId };

        Context.AddRange(item1, item2);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = (await repository.GetTransferActItemsByTransferActIdAsync(actId, CancellationToken.None)).ToList();

        // Assert
        result.Should().HaveCount(2);
    }

    /// <summary>
    /// Если позиций у акта нет, возвращает пустую коллекцию
    /// </summary>
    [Fact]
    public async Task GetTransferActItemsByTransferActIdAsyncShouldReturnEmpty()
    {
        // Act
        var result = await repository.GetTransferActItemsByTransferActIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Добавление позиции сохраняет её в базе
    /// </summary>
    [Fact]
    public async Task AddShouldSaveTransferActItemToDatabase()
    {
        // Arrange
        var item = new TransferActItem
        {
            Name = "Новая позиция",
            StockNumber = "ST-NEW",
            Unit = "шт",
            Quantity = 10,
            UnitPrice = 5000
        };

        // Act
        repository.Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Assert
        var saved = await repository.GetTransferActItemByStockNumberAsync("ST-NEW", CancellationToken.None);
        saved.Should().NotBeNull();
        saved!.Name.Should().Be("Новая позиция");
    }

    /// <summary>
    /// Обновление позиции изменяет её данные
    /// </summary>
    [Fact]
    public async Task UpdateShouldChangeTransferActItemData()
    {
        // Arrange
        var item = new TransferActItem
        {
            Name = "Старое название",
            StockNumber = "ST-UPD",
            Unit = "шт",
            Quantity = 1,
            UnitPrice = 1000
        };
        Context.Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        item.Name = "Новое название";
        repository.Update(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Assert
        var updated = await repository.GetTransferActItemByIdAsync(item.Id, CancellationToken.None);
        updated.Should().NotBeNull();
        updated!.Name.Should().Be("Новое название");
    }
}
