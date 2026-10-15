using AutoMapper;
using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Entities;
using OfficeProcurement.Repositories.Contracts;
using OfficeProcurement.Services.Contracts;
using OfficeProcurement.Services.Contracts.Exceptions;
using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Services;

/// <summary>
/// Сервис для работы с договорами
/// </summary>
public class ContractService : IContractService
{
    private readonly IContractRepository contractRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр сервиса договоров
    /// </summary>
    public ContractService(IContractRepository contractRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.contractRepository = contractRepository;
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    /// <summary>
    /// Получает список всех договоров
    /// </summary>
    async Task<IReadOnlyCollection<ContractModel>> IContractService.GetContracts(CancellationToken cancellationToken)
    {
        var result = await contractRepository.GetContractAsync(cancellationToken);
        return mapper.Map<IReadOnlyCollection<ContractModel>>(result);
    }

    /// <summary>
    /// Получает договор по идентификатору
    /// </summary>
    async Task<ContractModel> IContractService.GetContractById(Guid id, CancellationToken cancellationToken)
    {
        var result = await contractRepository.GetContractByIdAsync(id, cancellationToken);
        if (result is null)
        {
            throw new EntityNotFoundException<Contract>(id);
        }
        return mapper.Map<ContractModel>(result);
    }

    /// <summary>
    /// Создаёт новый договор
    /// </summary>
    async Task<ContractModel> IContractService.CreateContract(ContractCreateModel contractCreateModel, CancellationToken cancellationToken)
    {
        var existing = await contractRepository.GetContractByNumberAsync(contractCreateModel.Number, cancellationToken);
        if (existing is not null)
        {
            throw new OfficeProcurementInvalidOperationException($"Договор с номером '{contractCreateModel.Number}' уже существует");
        }
        var entity = mapper.Map<Contract>(contractCreateModel);
        contractRepository.Add(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var result = mapper.Map<ContractModel>(entity);
        return result;
    }

    /// <summary>
    /// Обновляет данные договора
    /// </summary>
    async Task IContractService.UpdateContract(ContractModel contractModel, CancellationToken cancellationToken)
    {
        var existing = await contractRepository.GetContractByNumberAsync(contractModel.Number, cancellationToken);
        if (existing is not null && existing.Id != contractModel.Id)
        {
            throw new OfficeProcurementInvalidOperationException($"Договор с номером '{contractModel.Number}' уже существует");
        }
        var entity = await contractRepository.GetContractByIdAsync(contractModel.Id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<Contract>(contractModel.Id);
        }
        mapper.Map(contractModel, entity);
        contractRepository.Update(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Удаляет договор по идентификатору
    /// </summary>
    async Task IContractService.DeleteContract(Guid id, CancellationToken cancellationToken)
    {
        var entity = await contractRepository.GetContractByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<Contract>(id);
        }
        contractRepository.Delete(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
