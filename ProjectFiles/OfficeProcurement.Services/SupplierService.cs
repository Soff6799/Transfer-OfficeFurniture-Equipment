using AutoMapper;
using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Entities;
using OfficeProcurement.Repositories.Contracts;
using OfficeProcurement.Services.Contracts;
using OfficeProcurement.Services.Contracts.Exceptions;
using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Services;

/// <summary>
/// Сервис для работы с поставщиками
/// </summary>
public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository supplierRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр сервиса поставщиков
    /// </summary>
    public SupplierService(ISupplierRepository supplierRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.supplierRepository = supplierRepository;
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    /// <summary>
    /// Получает список всех поставщиков
    /// </summary>
    async Task<IReadOnlyCollection<SupplierModel>> ISupplierService.GetSuppliers(CancellationToken cancellationToken)
    {
        var result = await supplierRepository.GetSupplierAsync(cancellationToken);
        return mapper.Map<IReadOnlyCollection<SupplierModel>>(result);
    }

    /// <summary>
    /// Получает поставщика по идентификатору
    /// </summary>
    async Task<SupplierModel> ISupplierService.GetSupplierById(Guid id, CancellationToken cancellationToken)
    {
        var result = await supplierRepository.GetSupplierByIdAsync(id, cancellationToken);
        if (result is null)
        {
            throw new EntityNotFoundException<Supplier>(id);
        }
        return mapper.Map<SupplierModel>(result);
    }

    /// <summary>
    /// Создаёт нового поставщика
    /// </summary>
    async Task<SupplierModel> ISupplierService.CreateSupplier(SupplierCreateModel supplierCreateModel, CancellationToken cancellationToken)
    {
        var existing = await supplierRepository.GetSupplierByCompanyNameAsync(supplierCreateModel.CompanyName, cancellationToken);

        if (existing is not null)
        {
            throw new OfficeProcurementInvalidOperationException($"Поставщик с названием '{supplierCreateModel.CompanyName}' уже существует");
        }
        var entity = mapper.Map<Supplier>(supplierCreateModel);
        supplierRepository.Add(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var result = mapper.Map<SupplierModel>(entity);
        return result;
    }

    /// <summary>
    /// Обновляет данные поставщика
    /// </summary>
    async Task ISupplierService.UpdateSupplier(SupplierModel supplierModel, CancellationToken cancellationToken)
    {
        var existing = await supplierRepository.GetSupplierByCompanyNameAsync(supplierModel.CompanyName, cancellationToken);

        if (existing is not null && existing.Id != supplierModel.Id)
        {
            throw new OfficeProcurementInvalidOperationException($"Поставщик с названием '{supplierModel.CompanyName}' уже существует");
        }
        var entity = await supplierRepository.GetSupplierByIdAsync(supplierModel.Id, cancellationToken);

        if (entity is null)
        {
            throw new EntityNotFoundException<Supplier>(supplierModel.Id);
        }
        mapper.Map(supplierModel, entity);
        supplierRepository.Update(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Удаляет поставщика по идентификатору
    /// </summary>
    async Task ISupplierService.DeleteSupplier(Guid id, CancellationToken cancellationToken)
    {
        var entity = await supplierRepository.GetSupplierByIdAsync(id, cancellationToken);

        if (entity is null)
        {
            throw new EntityNotFoundException<Supplier>(id);
        }
        supplierRepository.Delete(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
