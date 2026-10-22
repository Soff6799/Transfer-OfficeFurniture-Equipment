using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfficeProcurement.Common;
using OfficeProcurement.Models.TransferActItem;
using OfficeProcurement.Services.Contracts.Interface;
using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Controllers;

/// <summary>
/// Работа с позициями актов передачи
/// </summary>
[ApiController]
[Route("[controller]")]
public class TransferActItemsController : ControllerBase
{
    private readonly ITransferActItemService transferActItemService;
    private readonly IMapper mapper;
    private readonly IValidateService validateService;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера позиций актов передачи
    /// </summary>
    public TransferActItemsController(ITransferActItemService transferActItemService, IMapper mapper, IValidateService validateService)
    {
        this.transferActItemService = transferActItemService;
        this.mapper = mapper;
        this.validateService = validateService;
    }

    /// <summary>
    /// Получает список всех позиций актов передачи
    /// </summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<TransferActItemApiModel>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var items = await transferActItemService.GetTransferActItems(cancellationToken);
        return Ok(mapper.Map<IReadOnlyCollection<TransferActItemApiModel>>(items));
    }

    /// <summary>
    /// Получает позицию акта передачи по идентификатору
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<TransferActItemApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var item = await transferActItemService.GetTransferActItemById(id, cancellationToken);
        return Ok(mapper.Map<TransferActItemApiModel>(item));
    }

    /// <summary>
    /// Создаёт новую позицию акта передачи
    /// </summary>
    [HttpPost]
    [ProducesResponseType<TransferActItemApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] TransferActItemCreateApiModel createModel, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<TransferActItemCreateModel>(createModel);
        await validateService.ValidateAsync(mapped, cancellationToken);
        var result = await transferActItemService.CreateTransferActItem(mapped, cancellationToken);
        return Ok(mapper.Map<TransferActItemApiModel>(result));
    }

    /// <summary>
    /// Обновляет данные позиции акта передачи по идентификатору
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] TransferActItemCreateApiModel updateModel, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<TransferActItemModel>(updateModel);
        await validateService.ValidateAsync(mapped, cancellationToken);
        mapped.Id = id;
        await transferActItemService.UpdateTransferActItem(mapped, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Удаляет позицию акта передачи по идентификатору
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await transferActItemService.DeleteTransferActItem(id, cancellationToken);
        return NoContent();
    }
}
