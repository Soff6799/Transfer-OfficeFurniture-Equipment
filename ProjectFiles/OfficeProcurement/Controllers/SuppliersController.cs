using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfficeProcurement.Common;
using OfficeProcurement.Models.Supplier;
using OfficeProcurement.Services.Contracts.Interface;
using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Controllers;

/// <summary>
/// Работа с поставщиками
/// </summary>
[ApiController]
[Route("[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService supplierService;
    private readonly IMapper mapper;
    private readonly IValidateService validateService;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера поставщиков
    /// </summary>
    public SuppliersController(ISupplierService supplierService, IMapper mapper, IValidateService validateService)
    {
        this.supplierService = supplierService;
        this.mapper = mapper;
        this.validateService = validateService;
    }

    /// <summary>
    /// Получает список всех поставщиков
    /// </summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<SupplierApiModel>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var suppliers = await supplierService.GetSuppliers(cancellationToken);
        return Ok(mapper.Map<IReadOnlyCollection<SupplierApiModel>>(suppliers));
    }

    /// <summary>
    /// Получает поставщика по идентификатору
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<SupplierApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var supplier = await supplierService.GetSupplierById(id, cancellationToken);
        return Ok(mapper.Map<SupplierApiModel>(supplier));
    }

    /// <summary>
    /// Создаёт нового поставщика
    /// </summary>
    [HttpPost]
    [ProducesResponseType<SupplierApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] SupplierCreateApiModel createModel, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<SupplierCreateModel>(createModel);
        await validateService.ValidateAsync(mapped, cancellationToken);
        var result = await supplierService.CreateSupplier(mapped, cancellationToken);
        return Ok(mapper.Map<SupplierApiModel>(result));
    }

    /// <summary>
    /// Обновляет данные поставщика по идентификатору
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] SupplierCreateApiModel updateModel, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<SupplierModel>(updateModel);
        await validateService.ValidateAsync(mapped, cancellationToken);
        mapped.Id = id;
        await supplierService.UpdateSupplier(mapped, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Удаляет поставщика по идентификатору
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await supplierService.DeleteSupplier(id, cancellationToken);
        return NoContent();
    }
}
