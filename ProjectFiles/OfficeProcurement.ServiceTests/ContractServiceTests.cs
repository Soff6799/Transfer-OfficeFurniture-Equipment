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
/// Тесты для <see cref="ContractService"/>
/// </summary>
public class ContractServiceTests : BaseServiceTests
{
    private readonly IContractService contractService;
    private readonly IContractRepository contractRepository;

    /// <summary>
    /// Инициализирует тесты сервиса договоров
    /// </summary>
    public ContractServiceTests()
    {
        contractRepository = new ContractRepository(WriterContext, Context);
        contractService = new ContractService(contractRepository, UnitOfWork, Mapper);
    }

    /// <summary>
    /// Возвращает пустую коллекцию договоров
    /// </summary>
    [Fact]
    public async Task GetContractsShouldReturnEmptyCollection()
    {
        // Act
        var result = await contractService.GetContracts(CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Возвращает список договоров
    /// </summary>
    [Fact]
    public async Task GetContractsShouldReturnContracts()
    {
        // Arrange
        var contract1 = new Contract { Number = "Д-001", SignedAt = DateTimeOffset.Now, Type = ContractType.Supply };
        var contract2 = new Contract { Number = "Д-002", SignedAt = DateTimeOffset.Now, Type = ContractType.Rent };
        AddRange(contract1, contract2);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await contractService.GetContracts(CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
    }

    /// <summary>
    /// Возвращает договор по идентификатору
    /// </summary>
    [Fact]
    public async Task GetContractByIdShouldReturnValue()
    {
        // Arrange
        var entity = new Contract
        {
            Id = Guid.NewGuid(),
            Number = "Д-100",
            SignedAt = DateTimeOffset.Now,
            Type = ContractType.Supply
        };
        Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await contractService.GetContractById(entity.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(entity.Id);
        result.Number.Should().Be("Д-100");
    }

    /// <summary>
    /// Если договора нет, выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task GetContractByIdShouldThrowException()
    {
        // Act & Assert
        var act = () => contractService.GetContractById(Guid.NewGuid(), CancellationToken.None);
        await act.Should().ThrowAsync<EntityNotFoundException<Contract>>();
    }

    /// <summary>
    /// Создаёт новый договор
    /// </summary>
    [Fact]
    public async Task CreateContractShouldSaveToDatabase()
    {
        // Arrange
        var model = new ContractCreateModel
        {
            Number = "Д-NEW",
            Date = DateOnly.FromDateTime(DateTime.Now),
            Type = ContractType.Supply
        };

        // Act
        var result = await contractService.CreateContract(model, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Number.Should().Be("Д-NEW");

        var saved = await contractRepository.GetContractByNumberAsync("Д-NEW", CancellationToken.None);
        saved.Should().NotBeNull();
    }

    /// <summary>
    /// Если договор с таким номером уже существует, выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task CreateContractShouldThrowExceptionIfNumberExists()
    {
        // Arrange
        var existing = new Contract { Number = "Д-100", SignedAt = DateTimeOffset.Now, Type = ContractType.Supply };
        Add(existing);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var model = new ContractCreateModel
        {
            Number = "Д-100",
            Date = DateOnly.FromDateTime(DateTime.Now),
            Type = ContractType.Rent
        };

        // Act & Assert
        var act = () => contractService.CreateContract(model, CancellationToken.None);
        await act.Should().ThrowAsync<OfficeProcurementInvalidOperationException>();
    }

    /// <summary>
    /// Обновляет данные договора
    /// </summary>
    [Fact]
    public async Task UpdateContractShouldChangeData()
    {
        // Arrange
        var entity = new Contract
        {
            Id = Guid.NewGuid(),
            Number = "Д-OLD",
            SignedAt = DateTimeOffset.Now,
            Type = ContractType.Rent
        };
        Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var model = new ContractModel
        {
            Id = entity.Id,
            Number = "Д-UPDATED",
            Date = DateOnly.FromDateTime(DateTime.Now),
            Type = ContractType.Supply
        };

        // Act
        await contractService.UpdateContract(model, CancellationToken.None);

        // Assert
        var updated = await contractRepository.GetContractByIdAsync(entity.Id, CancellationToken.None);
        updated.Should().NotBeNull();
        updated!.Number.Should().Be("Д-UPDATED");
        updated.Type.Should().Be(ContractType.Supply);
    }

    /// <summary>
    /// Если договора нет, обновление выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task UpdateContractShouldThrowExceptionIfNotFound()
    {
        // Arrange
        var model = new ContractModel
        {
            Id = Guid.NewGuid(),
            Number = "Д-NOTEXIST",
            Date = DateOnly.FromDateTime(DateTime.Now),
            Type = ContractType.Supply
        };

        // Act & Assert
        var act = () => contractService.UpdateContract(model, CancellationToken.None);
        await act.Should().ThrowAsync<EntityNotFoundException<Contract>>();
    }

    /// <summary>
    /// Если номер уже занят другим договором, обновление выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task UpdateContractShouldThrowExceptionIfNumberExists()
    {
        // Arrange
        var entity1 = new Contract
        {
            Id = Guid.NewGuid(),
            Number = "Д-001",
            SignedAt = DateTimeOffset.Now,
            Type = ContractType.Rent
        };
        var entity2 = new Contract
        {
            Id = Guid.NewGuid(),
            Number = "Д-002",
            SignedAt = DateTimeOffset.Now,
            Type = ContractType.Supply
        };
        AddRange(entity1, entity2);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var model = new ContractModel
        {
            Id = entity1.Id,
            Number = "Д-002",
            Date = DateOnly.FromDateTime(DateTime.Now),
            Type = ContractType.Supply
        };

        // Act & Assert
        var act = () => contractService.UpdateContract(model, CancellationToken.None);
        await act.Should().ThrowAsync<OfficeProcurementInvalidOperationException>();
    }

    /// <summary>
    /// Удаляет договор
    /// </summary>
    [Fact]
    public async Task DeleteContractShouldMarkAsDeleted()
    {
        // Arrange
        var entity = new Contract
        {
            Id = Guid.NewGuid(),
            Number = "Д-DEL",
            SignedAt = DateTimeOffset.Now,
            Type = ContractType.Supply
        };
        Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        await contractService.DeleteContract(entity.Id, CancellationToken.None);

        // Assert
        var deleted = await contractRepository.GetContractByIdAsync(entity.Id, CancellationToken.None);
        deleted.Should().BeNull();
    }

    /// <summary>
    /// Если договора нет, удаление выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task DeleteContractShouldThrowExceptionIfNotFound()
    {
        // Act & Assert
        var act = () => contractService.DeleteContract(Guid.NewGuid(), CancellationToken.None);
        await act.Should().ThrowAsync<EntityNotFoundException<Contract>>();
    }
}
