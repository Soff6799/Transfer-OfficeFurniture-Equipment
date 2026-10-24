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
/// Тесты для <see cref="SupplierService"/>
/// </summary>
public class SupplierServiceTests : BaseServiceTests
{
    private readonly ISupplierService supplierService;
    private readonly ISupplierRepository supplierRepository;

    /// <summary>
    /// Инициализирует тесты сервиса поставщиков
    /// </summary>
    public SupplierServiceTests()
    {
        supplierRepository = new SupplierRepository(WriterContext, Context);
        supplierService = new SupplierService(supplierRepository, UnitOfWork, Mapper);
    }

    /// <summary>
    /// Возвращает пустую коллекцию поставщиков
    /// </summary>
    [Fact]
    public async Task GetSuppliersShouldReturnEmptyCollection()
    {
        // Act
        var result = await supplierService.GetSuppliers(CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Возвращает список поставщиков
    /// </summary>
    [Fact]
    public async Task GetSuppliersShouldReturnSuppliers()
    {
        // Arrange
        var supplier1 = new Supplier { CompanyName = "ООО Ромашка", Inn = "7701234567" };
        var supplier2 = new Supplier { CompanyName = "ООО Василёк", Inn = "7707654321" };
        AddRange(supplier1, supplier2);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await supplierService.GetSuppliers(CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
    }

    /// <summary>
    /// Возвращает поставщика по идентификатору
    /// </summary>
    [Fact]
    public async Task GetSupplierByIdShouldReturnValue()
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
        var result = await supplierService.GetSupplierById(entity.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(entity.Id);
        result.CompanyName.Should().Be("ООО Тест");
    }

    /// <summary>
    /// Если поставщика нет, выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task GetSupplierByIdShouldThrowException()
    {
        // Act & Assert
        var act = () => supplierService.GetSupplierById(Guid.NewGuid(), CancellationToken.None);
        await act.Should().ThrowAsync<EntityNotFoundException<Supplier>>();
    }

    /// <summary>
    /// Создаёт нового поставщика
    /// </summary>
    [Fact]
    public async Task CreateSupplierShouldSaveToDatabase()
    {
        // Arrange
        var model = new SupplierCreateModel
        {
            CompanyName = "ООО Новый",
            Inn = "7709999999",
            ContactLastName = "Петров",
            ContactName = "Пётр"
        };

        // Act
        var result = await supplierService.CreateSupplier(model, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CompanyName.Should().Be("ООО Новый");
        result.Inn.Should().Be("7709999999");

        // Проверяем, что поставщик сохранён в БД
        var saved = await supplierRepository.GetSupplierByInnAsync("7709999999", CancellationToken.None);
        saved.Should().NotBeNull();
    }

    /// <summary>
    /// Если поставщик с таким ИНН уже существует, выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task CreateSupplierShouldThrowExceptionIfInnExists()
    {
        // Arrange
        var existing = new Supplier { CompanyName = "ООО Старый", Inn = "7701234567" };
        Add(existing);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var model = new SupplierCreateModel
        {
            CompanyName = "ООО Новый",
            Inn = "7701234567"  // Тот же ИНН
        };

        // Act & Assert
        var act = () => supplierService.CreateSupplier(model, CancellationToken.None);
        await act.Should().ThrowAsync<OfficeProcurementInvalidOperationException>();
    }

    /// <summary>
    /// Обновляет данные поставщика
    /// </summary>
    [Fact]
    public async Task UpdateSupplierShouldChangeData()
    {
        // Arrange
        var entity = new Supplier
        {
            Id = Guid.NewGuid(),
            CompanyName = "ООО Старое",
            Inn = "7701234567"
        };
        Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var model = new SupplierModel
        {
            Id = entity.Id,
            CompanyName = "ООО Новое",
            Inn = "7701234567"
        };

        // Act
        await supplierService.UpdateSupplier(model, CancellationToken.None);

        // Assert
        var updated = await supplierRepository.GetSupplierByIdAsync(entity.Id, CancellationToken.None);
        updated.Should().NotBeNull();
        updated!.CompanyName.Should().Be("ООО Новое");
    }

    /// <summary>
    /// Если поставщика нет, обновление выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task UpdateSupplierShouldThrowExceptionIfNotFound()
    {
        // Arrange
        var model = new SupplierModel
        {
            Id = Guid.NewGuid(),
            CompanyName = "ООО Несуществующий",
            Inn = "7709999999"
        };

        // Act & Assert
        var act = () => supplierService.UpdateSupplier(model, CancellationToken.None);
        await act.Should().ThrowAsync<EntityNotFoundException<Supplier>>();
    }

    /// <summary>
    /// Если ИНН уже занят другим поставщиком, обновление выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task UpdateSupplierShouldThrowExceptionIfInnExists()
    {
        // Arrange
        var entity1 = new Supplier
        {
            Id = Guid.NewGuid(),
            CompanyName = "ООО Первый",
            Inn = "7701111111"
        };
        var entity2 = new Supplier
        {
            Id = Guid.NewGuid(),
            CompanyName = "ООО Второй",
            Inn = "7702222222"
        };
        AddRange(entity1, entity2);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var model = new SupplierModel
        {
            Id = entity1.Id,
            CompanyName = "ООО Первый обновлённый",
            Inn = "7702222222"  // ИНН уже занят entity2
        };

        // Act & Assert
        var act = () => supplierService.UpdateSupplier(model, CancellationToken.None);
        await act.Should().ThrowAsync<OfficeProcurementInvalidOperationException>();
    }

    /// <summary>
    /// Удаляет поставщика
    /// </summary>
    [Fact]
    public async Task DeleteSupplierShouldMarkAsDeleted()
    {
        // Arrange
        var entity = new Supplier
        {
            Id = Guid.NewGuid(),
            CompanyName = "ООО Удаляемый",
            Inn = "7708888888"
        };
        Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        await supplierService.DeleteSupplier(entity.Id, CancellationToken.None);

        // Assert
        var deleted = await supplierRepository.GetSupplierByIdAsync(entity.Id, CancellationToken.None);
        deleted.Should().BeNull();  // Soft delete
    }

    /// <summary>
    /// Если поставщика нет, удаление выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task DeleteSupplierShouldThrowExceptionIfNotFound()
    {
        // Act & Assert
        var act = () => supplierService.DeleteSupplier(Guid.NewGuid(), CancellationToken.None);
        await act.Should().ThrowAsync<EntityNotFoundException<Supplier>>();
    }
}
