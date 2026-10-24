using FluentAssertions;
using OfficeProcurement.Entities;
using OfficeProcurement.Repositories.Contracts;

namespace OfficeProcurement.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="ClientRepository"/>
/// </summary>
public class ClientRepositoryTests : BaseRepositoryTests
{
    private readonly IClientRepository repository;

    /// <summary>
    /// Инициализирует тесты репозитория клиентов
    /// </summary>
    public ClientRepositoryTests()
    {
        repository = new ClientRepository(WriterContext, Context);
    }

    /// <summary>
    /// Возвращает пустую коллекцию клиентов
    /// </summary>
    [Fact]
    public async Task GetClientAsyncShouldReturnEmptyCollection()
    {
        // Act
        var result = await repository.GetClientAsync(CancellationToken.None);

        // Assert
        result.Should().BeEmpty().And.NotBeNull();
    }

    /// <summary>
    /// Возвращает клиента по идентификатору если он существует
    /// </summary>
    [Fact]
    public async Task GetClientByIdAsyncShouldReturnValue()
    {
        // Arrange
        var entity = new Client
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
        var result = await repository.GetClientByIdAsync(entity.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(entity.Id);
    }

    /// <summary>
    /// Если клиента нет, возвращает null
    /// </summary>
    [Fact]
    public async Task GetClientByIdAsyncShouldReturnNull()
    {
        // Act
        var result = await repository.GetClientByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Если клиент удалён, возвращает null
    /// </summary>
    [Fact]
    public async Task GetClientByIdAsyncShouldReturnNullForDeleted()
    {
        // Arrange
        var item = new Client
        {
            Id = Guid.NewGuid(),
            CompanyName = "ООО Удалённый",
            Inn = "7709999999",
            DeletedAt = DateTimeOffset.Now
        };
        Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetClientByIdAsync(item.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает клиента по ИНН
    /// </summary>
    [Fact]
    public async Task GetClientByInnAsyncShouldReturnValue()
    {
        // Arrange
        var item = new Client
        {
            CompanyName = "ООО Ромашка",
            Inn = "7701234567"
        };
        Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetClientByInnAsync("7701234567", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Inn.Should().Be("7701234567");
    }

    /// <summary>
    /// Если клиента с таким ИНН нет, возвращает null
    /// </summary>
    [Fact]
    public async Task GetClientByInnAsyncShouldReturnNull()
    {
        // Act
        var result = await repository.GetClientByInnAsync("9999999999", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Если клиент удалён, по ИНН возвращает null
    /// </summary>
    [Fact]
    public async Task GetClientByInnAsyncShouldReturnNullForDeleted()
    {
        // Arrange
        var item = new Client
        {
            CompanyName = "ООО Удалённый",
            Inn = "7708888888",
            DeletedAt = DateTimeOffset.Now
        };
        Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetClientByInnAsync("7708888888", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает клиентов отсортированных по названию компании
    /// </summary>
    [Fact]
    public async Task GetClientAsyncShouldReturnSortedByCompanyName()
    {
        // Arrange
        var client1 = new Client { CompanyName = "Раз", Inn = "7700000001" };
        var client2 = new Client { CompanyName = "Два", Inn = "7700000002" };
        var client3 = new Client { CompanyName = "Три", Inn = "7700000003" };

        AddRange(client1, client2, client3);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = (await repository.GetClientAsync(CancellationToken.None)).ToList();

        // Assert
        result.Should().HaveCount(3);
        result[0].CompanyName.Should().Be("Два");
        result[1].CompanyName.Should().Be("Раз");
        result[2].CompanyName.Should().Be("Три");
    }

    /// <summary>
    /// Добавление клиента сохраняет его в базе
    /// </summary>
    [Fact]
    public async Task AddShouldSaveClientToDatabase()
    {
        // Arrange
        var client = new Client
        {
            CompanyName = "ООО Новый",
            Inn = "7709999999",
            ContactLastName = "Петров",
            ContactName = "Пётр"
        };

        // Act
        repository.Add(client);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Assert
        var saved = await repository.GetClientByInnAsync("7709999999", CancellationToken.None);
        saved.Should().NotBeNull();
        saved!.CompanyName.Should().Be("ООО Новый");
    }

    /// <summary>
    /// Обновление клиента изменяет его данные
    /// </summary>
    [Fact]
    public async Task UpdateShouldChangeClientData()
    {
        // Arrange
        var client = new Client
        {
            CompanyName = "ООО Старое",
            Inn = "7706666666",
            ContactLastName = "Иванов",
            ContactName = "Иван"
        };
        Add(client);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        client.CompanyName = "ООО Новое";
        repository.Update(client);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Assert
        var updated = await repository.GetClientByIdAsync(client.Id, CancellationToken.None);
        updated.Should().NotBeNull();
        updated!.CompanyName.Should().Be("ООО Новое");
    }
}
