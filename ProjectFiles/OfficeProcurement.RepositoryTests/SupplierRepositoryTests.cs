using FluentAssertions;
using OfficeProcurement.Entities;
using OfficeProcurement.Repositories.Contracts;
using Xunit;

namespace OfficeProcurement.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="SupplierRepository"/>
/// </summary>
public class SupplierRepositoryTests : BaseRepositoryTests
{
    private readonly ISupplierRepository repository;

    /// <summary>
    /// Инициализирует тесты репозитория поставщиков
    /// </summary>
    public SupplierRepositoryTests()
    {
        repository = new SupplierRepository(WriterContext, Context);
    }

    /// <summary>
    /// Возвращает пустую коллекцию поставщиков
    /// </summary>
    [Fact]
    public async Task GetSupplierAsyncShouldReturnEmptyCollection()
    {
        // Act
        var result = await repository.GetSupplierAsync(CancellationToken.None);

        // Assert
        result.Should().BeEmpty().And.NotBeNull();
    }

    /// <summary>
    /// Возвращает поставщиков отсортированных по названию компании
    /// </summary>
    [Fact]
    public async Task GetSupplierAsyncShouldReturnSortedByCompanyName()
    {
        // Arrange
        var supplier1 = new Supplier { CompanyName = "Бета", Inn = "7700000001" };
        var supplier2 = new Supplier { CompanyName = "Альфа", Inn = "7700000002" };
        var supplier3 = new Supplier { CompanyName = "Гамма", Inn = "7700000003" };

        AddRange(supplier1, supplier2, supplier3);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = (await repository.GetSupplierAsync(CancellationToken.None)).ToList();

        // Assert
        result.Should().HaveCount(3);
        result[0].CompanyName.Should().Be("Альфа");
        result[1].CompanyName.Should().Be("Бета");
        result[2].CompanyName.Should().Be("Гамма");
    }

    /// <summary>
    /// Возвращает поставщика по идентификатору если он существует
    /// </summary>
    [Fact]
    public async Task GetSupplierByIdAsyncShouldReturnValue()
    {
        // Arrange
        var entity = new Supplier
        {
            Id = Guid.NewGuid(),
            CompanyName = "ООО Тест",
            Inn = "7701234567",
            ContactLastName = "Иванов",
            ContactName = "Иван"
        };
        Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetSupplierByIdAsync(entity.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(entity.Id);
    }

    /// <summary>
    /// Если поставщика нет, возвращает null
    /// </summary>
    [Fact]
    public async Task GetSupplierByIdAsyncShouldReturnNull()
    {
        // Act
        var result = await repository.GetSupplierByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Если поставщик удалён, возвращает null
    /// </summary>
    [Fact]
    public async Task GetSupplierByIdAsyncShouldReturnNullForDeleted()
    {
        // Arrange
        var item = new Supplier
        {
            Id = Guid.NewGuid(),
            CompanyName = "ООО Удалённый",
            Inn = "7709999999",
            DeletedAt = DateTimeOffset.Now
        };
        Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetSupplierByIdAsync(item.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает поставщика по ИНН
    /// </summary>
    [Fact]
    public async Task GetSupplierByInnAsyncShouldReturnValue()
    {
        // Arrange
        var item = new Supplier
        {
            CompanyName = "ООО Ромашка",
            Inn = "7701234567"
        };
        Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetSupplierByInnAsync("7701234567", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Inn.Should().Be("7701234567");
    }

    /// <summary>
    /// Если поставщика с таким ИНН нет, возвращает null
    /// </summary>
    [Fact]
    public async Task GetSupplierByInnAsyncShouldReturnNull()
    {
        // Act
        var result = await repository.GetSupplierByInnAsync("9999999999", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Если поставщик удалён, по ИНН возвращает null
    /// </summary>
    [Fact]
    public async Task GetSupplierByInnAsyncShouldReturnNullForDeleted()
    {
        // Arrange
        var item = new Supplier
        {
            CompanyName = "ООО Удалённый",
            Inn = "7708888888",
            DeletedAt = DateTimeOffset.Now
        };
        Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetSupplierByInnAsync("7708888888", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Добавление поставщика сохраняет его в базе
    /// </summary>
    [Fact]
    public async Task AddShouldSaveSupplierToDatabase()
    {
        // Arrange
        var supplier = new Supplier
        {
            CompanyName = "ООО Новый",
            Inn = "7707777777",
            ContactLastName = "Петров",
            ContactName = "Пётр"
        };

        // Act
        repository.Add(supplier);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Assert
        var saved = await repository.GetSupplierByInnAsync("7707777777", CancellationToken.None);
        saved.Should().NotBeNull();
        saved!.CompanyName.Should().Be("ООО Новый");
    }

    /// <summary>
    /// Обновление поставщика изменяет его данные
    /// </summary>
    [Fact]
    public async Task UpdateShouldChangeSupplierData()
    {
        // Arrange
        var supplier = new Supplier
        {
            CompanyName = "ООО Старое",
            Inn = "7706666666"
        };
        Add(supplier);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        supplier.CompanyName = "ООО Новое";
        repository.Update(supplier);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Assert
        var updated = await repository.GetSupplierByIdAsync(supplier.Id, CancellationToken.None);
        updated.Should().NotBeNull();
        updated!.CompanyName.Should().Be("ООО Новое");
    }
}
