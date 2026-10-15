using AutoMapper;
using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Entities;
using OfficeProcurement.Repositories.Contracts;
using OfficeProcurement.Services.Contracts;
using OfficeProcurement.Services.Contracts.Exceptions;
using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Services;

/// <summary>
/// Сервис для работы с позициями акта передачи
/// </summary>
public class TransferActItemService : ITransferActItemService
{
    private readonly ITransferActItemRepository transferActItemRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр сервиса позиций акта передачи
    /// </summary>
    public TransferActItemService(ITransferActItemRepository transferActItemRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.transferActItemRepository = transferActItemRepository;
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    /// <summary>
    /// Получает список всех позиций акта передачи
    /// </summary>
    async Task<IReadOnlyCollection<TransferActItemModel>> ITransferActItemService.GetTransferActItems(CancellationToken cancellationToken)
    {
        var result = await transferActItemRepository.GetTransferActItemAsync(cancellationToken);

        return mapper.Map<IReadOnlyCollection<TransferActItemModel>>(result);
    }

    /// <summary>
    /// Получает позицию акта по идентификатору
    /// </summary>
    async Task<TransferActItemModel> ITransferActItemService.GetTransferActItemById(Guid id, CancellationToken cancellationToken)
    {
        var result = await transferActItemRepository.GetTransferActItemByIdAsync(id, cancellationToken);

        if (result is null)
        {
            throw new EntityNotFoundException<TransferActItem>(id);
        }
        return mapper.Map<TransferActItemModel>(result);
    }

    /// <summary>
    /// Создаёт новую позицию акта передачи
    /// </summary>
    async Task<TransferActItemModel> ITransferActItemService.CreateTransferActItem(TransferActItemCreateModel transferActItemCreateModel, CancellationToken cancellationToken)
    {
        var existing = await transferActItemRepository.GetTransferActItemByNameAsync(transferActItemCreateModel.Name, cancellationToken);
        if (existing is not null)
        {
            throw new OfficeProcurementInvalidOperationException($"Позиция с названием '{transferActItemCreateModel.Name}' уже существует");
        }
        var entity = mapper.Map<TransferActItem>(transferActItemCreateModel);
        transferActItemRepository.Add(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var result = mapper.Map<TransferActItemModel>(entity);
        return result;
    }

    /// <summary>
    /// Обновляет данные позиции акта
    /// </summary>
    async Task ITransferActItemService.UpdateTransferActItem(TransferActItemModel transferActItemModel, CancellationToken cancellationToken)
    {
        var existing = await transferActItemRepository.GetTransferActItemByNameAsync(transferActItemModel.Name, cancellationToken);
        if (existing is not null && existing.Id != transferActItemModel.Id)
        {
            throw new OfficeProcurementInvalidOperationException($"Позиция с названием '{transferActItemModel.Name}' уже существует");
        }
        var entity = await transferActItemRepository.GetTransferActItemByIdAsync(transferActItemModel.Id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<TransferActItem>(transferActItemModel.Id);
        }
        mapper.Map(transferActItemModel, entity);
        transferActItemRepository.Update(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Удаляет позицию акта по идентификатору
    async Task ITransferActItemService.DeleteTransferActItem(Guid id, CancellationToken cancellationToken)
    {
        var entity = await transferActItemRepository.GetTransferActItemByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<TransferActItem>(id);
        }
        transferActItemRepository.Delete(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
