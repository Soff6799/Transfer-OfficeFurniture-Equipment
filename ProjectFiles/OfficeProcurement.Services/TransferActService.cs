using AutoMapper;
using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Entities;
using OfficeProcurement.Repositories.Contracts;
using OfficeProcurement.Services.Contracts;
using OfficeProcurement.Services.Contracts.Exceptions;
using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Services;

/// <summary>
/// Сервис для работы с актами передачи
/// </summary>
public class TransferActService : ITransferActService
{
    private readonly ITransferActRepository transferActRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр сервиса актов передачи
    /// </summary>
    public TransferActService(ITransferActRepository transferActRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.transferActRepository = transferActRepository;
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    /// <summary>
    /// Получает список всех актов передачи
    /// </summary>
    async Task<IReadOnlyCollection<TransferActModel>> ITransferActService.GetTransferActs(CancellationToken cancellationToken)
    {
        var result = await transferActRepository.GetTransferActAsync(cancellationToken);
        return mapper.Map<IReadOnlyCollection<TransferActModel>>(result);
    }

    /// <summary>
    /// Получает акт передачи по идентификатору
    /// </summary>
    async Task<TransferActModel> ITransferActService.GetTransferActById(Guid id, CancellationToken cancellationToken)
    {
        var result = await transferActRepository.GetTransferActByIdAsync(id, cancellationToken);

        if (result is null)
        {
            throw new EntityNotFoundException<TransferAct>(id);
        }
        return mapper.Map<TransferActModel>(result);
    }

    /// <summary>
    /// Создаёт новый акт передачи
    /// </summary>
    async Task<TransferActModel> ITransferActService.CreateTransferAct(TransferActCreateModel transferActCreateModel, CancellationToken cancellationToken)
    {
        var existing = await transferActRepository.GetTransferActByNumberAsync(transferActCreateModel.Number, cancellationToken);

        if (existing is not null)
        {
            throw new OfficeProcurementInvalidOperationException($"Акт с номером '{transferActCreateModel.Number}' уже существует");
        }

        var entity = mapper.Map<TransferAct>(transferActCreateModel);
        transferActRepository.Add(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var result = mapper.Map<TransferActModel>(entity);
        return result;
    }

    /// <summary>
    /// Обновляет данные акта передачи
    /// </summary>
    async Task ITransferActService.UpdateTransferAct(TransferActModel transferActModel, CancellationToken cancellationToken)
    {
        var existing = await transferActRepository.GetTransferActByNumberAsync(transferActModel.Number, cancellationToken);

        if (existing is not null && existing.Id != transferActModel.Id)
        {
            throw new OfficeProcurementInvalidOperationException($"Акт с номером '{transferActModel.Number}' уже существует");
        }

        var entity = await transferActRepository.GetTransferActByIdAsync(transferActModel.Id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<TransferAct>(transferActModel.Id);
        }
        mapper.Map(transferActModel, entity);
        transferActRepository.Update(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Удаляет акт передачи по идентификатору
    /// </summary>
    async Task ITransferActService.DeleteTransferAct(Guid id, CancellationToken cancellationToken)
    {
        var entity = await transferActRepository.GetTransferActByIdAsync(id, cancellationToken);

        if (entity is null)
        {
            throw new EntityNotFoundException<TransferAct>(id);
        }

        transferActRepository.Delete(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
