using FluentAssertions;
using OfficeProcurement.Entities;
using OfficeProcurement.Repositories.Contracts;
using Xunit;

namespace OfficeProcurement.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="ProductRepository"/>
/// </summary>
public class ProductRepositoryTests : OfficeProcurementContextInMemory
{
    private readonly IProductRepository repository;

    /// <summary>
    /// Инициализирует тесты репозитория продуктов
    /// </summary>
    public ProductRepositoryTests()
    {
        repository = new ProductRepository(WriterContext, Context);
    }

    /// <summary>
    /// Возвращает пустую коллекцию продуктов
    /// </summary>
    [Fact]
    public async Task GetProductAsyncShouldReturnEmptyCollection()
    {
        // Act
        var result = await repository.GetProductAsync(CancellationToken.None);

        // Assert
        result.Should().BeEmpty().And.NotBeNull();
    }

    /// <summary>
    /// Возвращает продукты отсортированные по названию
    /// </summary>
    [Fact]
    public async Task GetProductAsyncShouldReturnSortedByName()
    {
        // Arrange
        var product1 = new Product { Name = "Стул", Unit = "шт" };
        var product2 = new Product { Name = "Стол", Unit = "шт" };
        var product3 = new Product { Name = "Шкаф", Unit = "шт" };

        Context.AddRange(product1, product2, product3);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = (await repository.GetProductAsync(CancellationToken.None)).ToList();

        // Assert
        result.Should().HaveCount(3);
        result[0].Name.Should().Be("Стол");
        result[1].Name.Should().Be("Стул");
        result[2].Name.Should().Be("Шкаф");
    }

    /// <summary>
    /// Возвращает продукт по идентификатору если он существует
    /// </summary>
    [Fact]
    public async Task GetProductByIdAsyncShouldReturnValue()
    {
        // Arrange
        var entity = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Стол офисный",
            StockNumber = "ST-001",
            Unit = "шт",
            Price = 15000
        };
        Context.Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetProductByIdAsync(entity.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(entity.Id);
    }

    /// <summary>
    /// Если продукта нет, возвращает null
    /// </summary>
    [Fact]
    public async Task GetProductByIdAsyncShouldReturnNull()
    {
        // Act
        var result = await repository.GetProductByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Если продукт удалён, возвращает null
    /// </summary>
    [Fact]
    public async Task GetProductByIdAsyncShouldReturnNullForDeleted()
    {
        // Arrange
        var item = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Удалённый",
            Unit = "шт",
            DeletedAt = DateTimeOffset.Now
        };
        Context.Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetProductByIdAsync(item.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает продукт по складскому номеру
    /// </summary>
    [Fact]
    public async Task GetProductByStockNumberAsyncShouldReturnValue()
    {
        // Arrange
        var item = new Product
        {
            Name = "Стол",
            StockNumber = "ST-100",
            Unit = "шт"
        };
        Context.Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetProductByStockNumberAsync("ST-100", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.StockNumber.Should().Be("ST-100");
    }

    /// <summary>
    /// Если продукта с таким складским номером нет, возвращает null
    /// </summary>
    [Fact]
    public async Task GetProductByStockNumberAsyncShouldReturnNull()
    {
        // Act
        var result = await repository.GetProductByStockNumberAsync("ST-999", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Если продукт удалён, по складскому номеру возвращает null
    /// </summary>
    [Fact]
    public async Task GetProductByStockNumberAsyncShouldReturnNullForDeleted()
    {
        // Arrange
        var item = new Product
        {
            Name = "Удалённый",
            StockNumber = "ST-DEL",
            Unit = "шт",
            DeletedAt = DateTimeOffset.Now
        };
        Context.Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetProductByStockNumberAsync("ST-DEL", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Добавление продукта сохраняет его в базе
    /// </summary>
    [Fact]
    public async Task AddShouldSaveProductToDatabase()
    {
        // Arrange
        var product = new Product
        {
            Name = "Новый продукт",
            StockNumber = "ST-NEW",
            Unit = "шт",
            Price = 5000
        };

        // Act
        repository.Add(product);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Assert
        var saved = await repository.GetProductByStockNumberAsync("ST-NEW", CancellationToken.None);
        saved.Should().NotBeNull();
        saved!.Name.Should().Be("Новый продукт");
    }

    /// <summary>
    /// Обновление продукта изменяет его данные
    /// </summary>
    [Fact]
    public async Task UpdateShouldChangeProductData()
    {
        // Arrange
        var product = new Product
        {
            Name = "Старое название",
            StockNumber = "ST-UPD",
            Unit = "шт",
            Price = 1000
        };
        Context.Add(product);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        product.Name = "Новое название";
        repository.Update(product);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Assert
        var updated = await repository.GetProductByIdAsync(product.Id, CancellationToken.None);
        updated.Should().NotBeNull();
        updated!.Name.Should().Be("Новое название");
    }
}
