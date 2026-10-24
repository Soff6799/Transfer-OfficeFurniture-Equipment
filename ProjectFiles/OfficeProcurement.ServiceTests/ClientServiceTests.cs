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
/// Тесты для <see cref="ClientService"/>
/// </summary>
public class ClientServiceTests : BaseServiceTests
{
    private readonly IClientService clientService;
    private readonly IClientRepository clientRepository;

    /// <summary>
    /// Инициализирует тесты сервиса клиентов
    /// </summary>
    public ClientServiceTests()
    {
        clientRepository = new ClientRepository(WriterContext, Context);
        clientService = new ClientService(clientRepository, UnitOfWork, Mapper);
    }

    /// <summary>
    /// Возвращает пустую коллекцию клиентов
    /// </summary>
    [Fact]
    public async Task GetClientsShouldReturnEmptyCollection()
    {
        // Act
        var result = await clientService.GetClients(CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Возвращает список клиентов
    /// </summary>
    [Fact]
    public async Task GetClientsShouldReturnClients()
    {
        // Arrange
        var client1 = new Client { CompanyName = "ООО Ромашка", Inn = "7701234567" };
        var client2 = new Client { CompanyName = "ООО Василёк", Inn = "7707654321" };
        AddRange(client1, client2);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await clientService.GetClients(CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
    }

    /// <summary>
    /// Возвращает клиента по идентификатору
    /// </summary>
    [Fact]
    public async Task GetClientByIdShouldReturnValue()
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
        var result = await clientService.GetClientById(entity.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(entity.Id);
        result.CompanyName.Should().Be("ООО Тест");
    }

    /// <summary>
    /// Если клиента нет, выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task GetClientByIdShouldThrowException()
    {
        // Act & Assert
        var act = () => clientService.GetClientById(Guid.NewGuid(), CancellationToken.None);
        await act.Should().ThrowAsync<EntityNotFoundException<Client>>();
    }

    /// <summary>
    /// Создаёт нового клиента
    /// </summary>
    [Fact]
    public async Task CreateClientShouldSaveToDatabase()
    {
        // Arrange
        var model = new ClientCreateModel
        {
            CompanyName = "ООО Новый",
            Inn = "7709999999",
            ContactLastName = "Петров",
            ContactName = "Пётр"
        };

        // Act
        var result = await clientService.CreateClient(model, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CompanyName.Should().Be("ООО Новый");
        result.Inn.Should().Be("7709999999");
        var saved = await clientRepository.GetClientByInnAsync("7709999999", CancellationToken.None);
        saved.Should().NotBeNull();
    }

    /// <summary>
    /// Если клиент с таким ИНН уже существует, выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task CreateClientShouldThrowExceptionIfInnExists()
    {
        // Arrange
        var existing = new Client { CompanyName = "ООО Старый", Inn = "7701234567" };
        Add(existing);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var model = new ClientCreateModel
        {
            CompanyName = "ООО Новый",
            Inn = "7701234567"
        };

        // Act & Assert
        var act = () => clientService.CreateClient(model, CancellationToken.None);
        await act.Should().ThrowAsync<OfficeProcurementInvalidOperationException>();
    }

    /// <summary>
    /// Обновляет данные клиента
    /// </summary>
    [Fact]
    public async Task UpdateClientShouldChangeData()
    {
        // Arrange
        var entity = new Client
        {
            Id = Guid.NewGuid(),
            CompanyName = "ООО Старое",
            Inn = "7701234567"
        };
        Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var model = new ClientModel
        {
            Id = entity.Id,
            CompanyName = "ООО Новое",
            Inn = "7701234567"
        };

        // Act
        await clientService.UpdateClient(model, CancellationToken.None);

        // Assert
        var updated = await clientRepository.GetClientByIdAsync(entity.Id, CancellationToken.None);
        updated.Should().NotBeNull();
        updated!.CompanyName.Should().Be("ООО Новое");
    }

    /// <summary>
    /// Если клиента нет, обновление выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task UpdateClientShouldThrowExceptionIfNotFound()
    {
        // Arrange
        var model = new ClientModel
        {
            Id = Guid.NewGuid(),
            CompanyName = "ООО Несуществующий",
            Inn = "7709999999"
        };

        // Act & Assert
        var act = () => clientService.UpdateClient(model, CancellationToken.None);
        await act.Should().ThrowAsync<EntityNotFoundException<Client>>();
    }

    /// <summary>
    /// Удаляет клиента
    /// </summary>
    [Fact]
    public async Task DeleteClientShouldMarkAsDeleted()
    {
        // Arrange
        var entity = new Client
        {
            Id = Guid.NewGuid(),
            CompanyName = "ООО Удаляемый",
            Inn = "7708888888"
        };
        Add(entity);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        await clientService.DeleteClient(entity.Id, CancellationToken.None);

        // Assert
        var deleted = await clientRepository.GetClientByIdAsync(entity.Id, CancellationToken.None);
        deleted.Should().BeNull();
    }

    /// <summary>
    /// Если клиента нет, удаление выбрасывает исключение
    /// </summary>
    [Fact]
    public async Task DeleteClientShouldThrowExceptionIfNotFound()
    {
        // Act & Assert
        var act = () => clientService.DeleteClient(Guid.NewGuid(), CancellationToken.None);
        await act.Should().ThrowAsync<EntityNotFoundException<Client>>();
    }
}
