using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfficeProcurement.Common;
using OfficeProcurement.Models.Product;
using OfficeProcurement.Services.Contracts.Interface;
using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Controllers;

/// <summary>
/// Работа с продуктами
/// </summary>
[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService productService;
    private readonly IMapper mapper;
    private readonly IValidateService validateService;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера продуктов
    /// </summary>
    public ProductsController(IProductService productService, IMapper mapper, IValidateService validateService)
    {
        this.productService = productService;
        this.mapper = mapper;
        this.validateService = validateService;
    }

    /// <summary>
    /// Получает список всех продуктов
    /// </summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<ProductApiModel>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var products = await productService.GetProducts(cancellationToken);
        return Ok(mapper.Map<IReadOnlyCollection<ProductApiModel>>(products));
    }

    /// <summary>
    /// Получает продукт по идентификатору
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<ProductApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var product = await productService.GetProductById(id, cancellationToken);
        return Ok(mapper.Map<ProductApiModel>(product));
    }

    /// <summary>
    /// Создаёт новый продукт
    /// </summary>
    [HttpPost]
    [ProducesResponseType<ProductApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] ProductCreateApiModel createModel, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<ProductCreateModel>(createModel);
        await validateService.ValidateAsync(mapped, cancellationToken);
        var result = await productService.CreateProduct(mapped, cancellationToken);
        return Ok(mapper.Map<ProductApiModel>(result));
    }

    /// <summary>
    /// Обновляет данные продукта по идентификатору
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ProductCreateApiModel updateModel, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<ProductModel>(updateModel);
        await validateService.ValidateAsync(mapped, cancellationToken);
        mapped.Id = id;
        await productService.UpdateProduct(mapped, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Удаляет продукт по идентификатору
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await productService.DeleteProduct(id, cancellationToken);
        return NoContent();
    }
}
