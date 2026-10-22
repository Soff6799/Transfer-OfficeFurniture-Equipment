using AutoMapper;
using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Entities;
using OfficeProcurement.Repositories.Contracts;
using OfficeProcurement.Services.Contracts.Exceptions;
using OfficeProcurement.Services.Contracts.Interface;
using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Services;

/// <summary>
/// Сервис для работы с продуктами
/// </summary>
public class ProductService : IProductService
{
    private readonly IProductRepository productRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр сервиса продуктов
    /// </summary>
    public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.productRepository = productRepository;
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    /// <summary>
    /// Получает список всех продуктов
    /// </summary>
    async Task<IReadOnlyCollection<ProductModel>> IProductService.GetProducts(CancellationToken cancellationToken)
    {
        var result = await productRepository.GetProductAsync(cancellationToken);
        return mapper.Map<IReadOnlyCollection<ProductModel>>(result);
    }

    /// <summary>
    /// Получает продукт по идентификатору
    /// </summary>
    async Task<ProductModel> IProductService.GetProductById(Guid id, CancellationToken cancellationToken)
    {
        var result = await productRepository.GetProductByIdAsync(id, cancellationToken);
        if (result is null)
        {
            throw new EntityNotFoundException<Product>(id);
        }
        return mapper.Map<ProductModel>(result);
    }

    /// <summary>
    /// Создаёт новый продукт
    /// </summary>
    async Task<ProductModel> IProductService.CreateProduct(ProductCreateModel productCreateModel, CancellationToken cancellationToken)
    {
        if (productCreateModel.StockNumber is not null)
        {
            var existing = await productRepository.GetProductByStockNumberAsync(productCreateModel.StockNumber, cancellationToken);
            if (existing is not null)
            {
                throw new OfficeProcurementInvalidOperationException($"Продукт со складским номером '{productCreateModel.StockNumber}' уже существует");
            }
        }
        var entity = mapper.Map<Product>(productCreateModel);
        productRepository.Add(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var result = mapper.Map<ProductModel>(entity);
        return result;
    }

    /// <summary>
    /// Обновляет данные продукта
    /// </summary>
    async Task IProductService.UpdateProduct(ProductModel productModel, CancellationToken cancellationToken)
    {
        if (productModel.StockNumber is not null)
        {
            var existing = await productRepository.GetProductByStockNumberAsync(productModel.StockNumber, cancellationToken);
            if (existing is not null && existing.Id != productModel.Id)
            {
                throw new OfficeProcurementInvalidOperationException($"Продукт со складским номером '{productModel.StockNumber}' уже существует");
            }
        }
        var entity = await productRepository.GetProductByIdAsync(productModel.Id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<Product>(productModel.Id);
        }
        mapper.Map(productModel, entity);
        productRepository.Update(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Удаляет продукт по идентификатору
    /// </summary>
    async Task IProductService.DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        var entity = await productRepository.GetProductByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            throw new EntityNotFoundException<Product>(id);
        }
        productRepository.Delete(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
