using FluentAssertions;
using OfficeProcurement.Entities;
using OfficeProcurement.Repositories.Contracts;
using Xunit;

namespace OfficeProcurement.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="ContractRepository"/>
/// </summary>
public class ContractRepositoryTests : OfficeProcurementContextInMemory
{
    private readonly IContractRepository repository;

    /// <summary>
    /// Инициализирует тесты репозитория договоров
    /// </summary>
    public ContractRepositoryTests()
    {
        repository = new ContractRepository(WriterContext, Context);
    }

    /// <summary>
    /// Возвращает пустую коллекцию договоров
    /// </summary>
    [Fact]
    public async Task GetContractAsyncShouldReturnEmptyCollection()
    {
        // Act
        var result = await repository.GetContractAsync(CancellationToken.None);

        // Assert
        result.Should().BeEmpty().And.NotBeNull();
    }

    /// <summary>
    /// Возвращает договоры отсортированные по номеру
    /// </summary>
    [Fact]
    public async Task GetContractAsyncShouldReturnSortedByNumber()
    {
        // Arrange
        var contract1 = new Contract { Number = "Д-003", SignedAt = DateTimeOffset.Now, Type = ContractType.Supply };
        var contract2 = new Contract { Number = "Д-001", SignedAt = DateTimeOffset.Now, Type = ContractType.Supply };
        var contract3 = new Contract { Number = "Д-002", SignedAt = DateTimeOffset.Now, Type = ContractType.Rent };

        Context.AddRange(contract1, contract2, contract3);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = (await repository.GetContractAsync(CancellationToken.None)).ToList();

        // Assert
        result.Should().HaveCount(3);
        result[0].Number.Should().Be("Д-001");
        result[1].Number.Should().Be("Д-002");
        result[2].Number.Should().Be("Д-003");
    }

    /// <summary>
    /// Возвращает договор по идентификатору если он существует
    /// </summary>
    [Fact]
    public async Task GetContractByIdAsyncShouldReturnValue()
    {
        // Arrange
        var entity = new Contract
        {
            Id = Guid.NewGuid(),
            Number = "Д-100",
            SignedAt = DateTimeOffset.Now,
            Type = ContractType.Supply
        };
        Context.Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetContractByIdAsync(entity.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(entity.Id);
    }

    /// <summary>
    /// Если договора нет, возвращает null
    /// </summary>
    [Fact]
    public async Task GetContractByIdAsyncShouldReturnNull()
    {
        // Act
        var result = await repository.GetContractByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Если договор удалён, возвращает null
    /// </summary>
    [Fact]
    public async Task GetContractByIdAsyncShouldReturnNullForDeleted()
    {
        // Arrange
        var item = new Contract
        {
            Id = Guid.NewGuid(),
            Number = "Д-002",
            SignedAt = DateTimeOffset.Now,
            Type = ContractType.Rent,
            DeletedAt = DateTimeOffset.Now
        };
        Context.Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetContractByIdAsync(item.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает договор по номеру
    /// </summary>
    [Fact]
    public async Task GetContractByNumberAsyncShouldReturnValue()
    {
        // Arrange
        var item = new Contract
        {
            Number = "Д-200",
            SignedAt = DateTimeOffset.Now,
            Type = ContractType.Supply
        };
        Context.Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetContractByNumberAsync("Д-200", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Number.Should().Be("Д-200");
    }

    /// <summary>
    /// Если договора с таким номером нет, возвращает null
    /// </summary>
    [Fact]
    public async Task GetContractByNumberAsyncShouldReturnNull()
    {
        // Act
        var result = await repository.GetContractByNumberAsync("Д-999", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Если договор удалён, по номеру возвращает null
    /// </summary>
    [Fact]
    public async Task GetContractByNumberAsyncShouldReturnNullForDeleted()
    {
        // Arrange
        var item = new Contract
        {
            Number = "Д-300",
            SignedAt = DateTimeOffset.Now,
            Type = ContractType.Rent,
            DeletedAt = DateTimeOffset.Now
        };
        Context.Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetContractByNumberAsync("Д-300", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Добавление договора сохраняет его в базе
    /// </summary>
    [Fact]
    public async Task AddShouldSaveContractToDatabase()
    {
        // Arrange
        var contract = new Contract
        {
            Number = "Д-NEW",
            SignedAt = DateTimeOffset.Now,
            Type = ContractType.Supply
        };

        // Act
        repository.Add(contract);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Assert
        var saved = await repository.GetContractByNumberAsync("Д-NEW", CancellationToken.None);
        saved.Should().NotBeNull();
        saved!.Number.Should().Be("Д-NEW");
    }

    /// <summary>
    /// Обновление договора изменяет его данные
    /// </summary>
    [Fact]
    public async Task UpdateShouldChangeContractData()
    {
        // Arrange
        var contract = new Contract
        {
            Number = "Д-OLD",
            SignedAt = DateTimeOffset.Now,
            Type = ContractType.Rent
        };
        Context.Add(contract);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        contract.Number = "Д-UPDATED";
        contract.Type = ContractType.Supply;
        repository.Update(contract);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Assert
        var updated = await repository.GetContractByIdAsync(contract.Id, CancellationToken.None);
        updated.Should().NotBeNull();
        updated!.Number.Should().Be("Д-UPDATED");
        updated.Type.Should().Be(ContractType.Supply);
    }
}
