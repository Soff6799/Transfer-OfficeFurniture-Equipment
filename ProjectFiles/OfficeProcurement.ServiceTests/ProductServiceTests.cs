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
/// Тесты для <see cref="ProductService"/>
/// </summary>
public class ProductServiceTests : BaseServiceTests
{
    private readonly IProductService productService;
    private readonly IProductRepository productRepository;

    /// <summary>
    /// Инициализирует тесты сервиса продуктов
    /// </summary>
    public ProductServiceTests()
    {
        productRepository = new ProductRepository(WriterContext, Context);
        productService = new ProductService(productRepository, UnitOfWork, Mapper);
    }

    /// <summary>
    /// Возвращает пустую коллекцию продуктов
    /// </summary>
    [Fact]
    public async Task GetProductsShouldReturnEmptyCollection()
    {
        // Act
        var result = await productService.GetProducts(CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Возвращает список продуктов
    /// </summary>
    [Fact]
    public async Task GetProductsShouldReturnProducts()
    {
        // Arrange
        var product1 = new Product { Name = "Стол", Unit = "шт" };
        var product2 = new Product { Name = "Стул", Unit = "шт" };
        AddRange(product1, product2);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await productService.GetProducts(CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
    }

    /// <summary>
    /// Возвращает продукт по идентификатору
    /// </summary>
    [Fact]
    public async Task GetProductByIdShouldReturnValue()
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
        Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await productService.GetProductById(entity.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(entity.Id);
        result.Name.Should().Be("Стол офисный");
    }

    /// <summary>
    /// Если продукта нет, выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task GetProductByIdShouldThrowException()
    {
        // Act & Assert
        var act = () => productService.GetProductById(Guid.NewGuid(), CancellationToken.None);
        await act.Should().ThrowAsync<EntityNotFoundException<Product>>();
    }

    /// <summary>
    /// Создаёт новый продукт
    /// </summary>
    [Fact]
    public async Task CreateProductShouldSaveToDatabase()
    {
        // Arrange
        var model = new ProductCreateModel
        {
            Name = "Новый продукт",
            StockNumber = "ST-NEW",
            Unit = "шт",
            UnitPrice = 5000
        };

        // Act
        var result = await productService.CreateProduct(model, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Новый продукт");
        result.StockNumber.Should().Be("ST-NEW");

        // Проверяем, что продукт сохранён в БД
        var saved = await productRepository.GetProductByStockNumberAsync("ST-NEW", CancellationToken.None);
        saved.Should().NotBeNull();
    }

    /// <summary>
    /// Создаёт продукт без складского номера
    /// </summary>
    [Fact]
    public async Task CreateProductWithoutStockNumberShouldSucceed()
    {
        // Arrange
        var model = new ProductCreateModel
        {
            Name = "Продукт без номера",
            StockNumber = null,  // Нет складского номера
            Unit = "шт",
            UnitPrice = 1000
        };

        // Act
        var result = await productService.CreateProduct(model, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Продукт без номера");
    }

    /// <summary>
    /// Если продукт с таким складским номером уже существует, выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task CreateProductShouldThrowExceptionIfStockNumberExists()
    {
        // Arrange
        var existing = new Product { Name = "Старый", StockNumber = "ST-100", Unit = "шт" };
        Add(existing);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var model = new ProductCreateModel
        {
            Name = "Новый",
            StockNumber = "ST-100",  // Тот же номер
            Unit = "шт",
            UnitPrice = 2000
        };

        // Act & Assert
        var act = () => productService.CreateProduct(model, CancellationToken.None);
        await act.Should().ThrowAsync<OfficeProcurementInvalidOperationException>();
    }

    /// <summary>
    /// Обновляет данные продукта
    /// </summary>
    [Fact]
    public async Task UpdateProductShouldChangeData()
    {
        // Arrange
        var entity = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Старое название",
            StockNumber = "ST-OLD",
            Unit = "шт",
            Price = 1000
        };
        Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var model = new ProductModel
        {
            Id = entity.Id,
            Name = "Новое название",
            StockNumber = "ST-OLD",
            Unit = "шт",
            UnitPrice = 2000
        };

        // Act
        await productService.UpdateProduct(model, CancellationToken.None);

        // Assert
        var updated = await productRepository.GetProductByIdAsync(entity.Id, CancellationToken.None);
        updated.Should().NotBeNull();
        updated!.Name.Should().Be("Новое название");
        updated.Price.Should().Be(2000);
    }

    /// <summary>
    /// Если продукта нет, обновление выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task UpdateProductShouldThrowExceptionIfNotFound()
    {
        // Arrange
        var model = new ProductModel
        {
            Id = Guid.NewGuid(),
            Name = "Несуществующий",
            StockNumber = "ST-NO",
            Unit = "шт",
            UnitPrice = 1000
        };

        // Act & Assert
        var act = () => productService.UpdateProduct(model, CancellationToken.None);
        await act.Should().ThrowAsync<EntityNotFoundException<Product>>();
    }

    /// <summary>
    /// Если складской номер уже занят другим продуктом, обновление выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task UpdateProductShouldThrowExceptionIfStockNumberExists()
    {
        // Arrange
        var entity1 = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Продукт 1",
            StockNumber = "ST-001",
            Unit = "шт"
        };
        var entity2 = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Продукт 2",
            StockNumber = "ST-002",
            Unit = "шт"
        };
        AddRange(entity1, entity2);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var model = new ProductModel
        {
            Id = entity1.Id,
            Name = "Продукт 1 обновлённый",
            StockNumber = "ST-002",  // Номер уже занят entity2
            Unit = "шт",
            UnitPrice = 1000
        };

        // Act & Assert
        var act = () => productService.UpdateProduct(model, CancellationToken.None);
        await act.Should().ThrowAsync<OfficeProcurementInvalidOperationException>();
    }

    /// <summary>
    /// Удаляет продукт
    /// </summary>
    [Fact]
    public async Task DeleteProductShouldMarkAsDeleted()
    {
        // Arrange
        var entity = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Удаляемый",
            StockNumber = "ST-DEL",
            Unit = "шт"
        };
        Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        await productService.DeleteProduct(entity.Id, CancellationToken.None);

        // Assert
        var deleted = await productRepository.GetProductByIdAsync(entity.Id, CancellationToken.None);
        deleted.Should().BeNull();  // Soft delete
    }

    /// <summary>
    /// Если продукта нет, удаление выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task DeleteProductShouldThrowExceptionIfNotFound()
    {
        // Act & Assert
        var act = () => productService.DeleteProduct(Guid.NewGuid(), CancellationToken.None);
        await act.Should().ThrowAsync<EntityNotFoundException<Product>>();
    }
}
