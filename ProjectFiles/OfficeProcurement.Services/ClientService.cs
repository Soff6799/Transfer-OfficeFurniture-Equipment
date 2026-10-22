using AutoMapper;
using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Entities;
using OfficeProcurement.Repositories.Contracts;
using OfficeProcurement.Services.Contracts.Exceptions;
using OfficeProcurement.Services.Contracts.Interface;
using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Services;

/// <summary>
/// Сервис для работы с клиентами
/// </summary>
public class ClientService : IClientService
{
    private readonly IClientRepository clientRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр сервиса клиентов
    /// </summary>
    public ClientService(IClientRepository clientRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.clientRepository = clientRepository;
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    /// <summary>
    /// Получает список всех клиентов
    /// </summary>
    async Task<IReadOnlyCollection<ClientModel>> IClientService.GetClients(CancellationToken cancellationToken)
    {
        var result = await clientRepository.GetClientAsync(cancellationToken);
        return mapper.Map<IReadOnlyCollection<ClientModel>>(result);
    }

    /// <summary>
    /// Получает клиента по идентификатору
    /// </summary>
    async Task<ClientModel> IClientService.GetClientById(Guid id, CancellationToken cancellationToken)
    {
        var result = await clientRepository.GetClientByIdAsync(id, cancellationToken);
        if (result is null)
        {
            throw new EntityNotFoundException<Client>(id);
        }
        return mapper.Map<ClientModel>(result);
    }

    /// <summary>
    /// Создаёт нового клиента
    /// </summary>
    /// <exception cref="OfficeProcurementInvalidOperationException">Клиент с таким названием уже существует</exception>
    async Task<ClientModel> IClientService.CreateClient(ClientCreateModel clientCreateModel, CancellationToken cancellationToken)
    {
        var existing = await clientRepository.GetClientByInnAsync(clientCreateModel.Inn, cancellationToken);
        if (existing is not null)
        {
            throw new OfficeProcurementInvalidOperationException($"Клиент с ИНН '{clientCreateModel.Inn}' уже существует");
        }
        var entity = mapper.Map<Client>(clientCreateModel);
        clientRepository.Add(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var result = mapper.Map<ClientModel>(entity);
        return result;
    }

    /// <summary>
    /// Обновляет данные клиента
    /// </summary>
    async Task IClientService.UpdateClient(ClientModel clientModel, CancellationToken cancellationToken)
    {
        var existingByInn = await clientRepository.GetClientByInnAsync(clientModel.Inn, cancellationToken);
        if (existingByInn is not null && existingByInn.Id != clientModel.Id)
        {
            throw new OfficeProcurementInvalidOperationException($"Клиент с ИНН '{clientModel.Inn}' уже существует");
        }
        var entity = await clientRepository.GetClientByIdAsync(clientModel.Id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<Client>(clientModel.Id);
        }
        mapper.Map(clientModel, entity);
        clientRepository.Update(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Удаляет клиента по идентификатору
    /// </summary>
    async Task IClientService.DeleteClient(Guid id, CancellationToken cancellationToken)
    {
        var entity = await clientRepository.GetClientByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<Client>(id);
        }
        clientRepository.Delete(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
