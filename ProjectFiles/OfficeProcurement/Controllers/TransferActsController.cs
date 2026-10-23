using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfficeProcurement.Common;
using OfficeProcurement.Models.TransferAct;
using OfficeProcurement.Services.Contracts.Interface;
using OfficeProcurement.Services.Contracts.Models;
using OfficeProcurement.Services.Implementations;

namespace OfficeProcurement.Controllers;

/// <summary>
/// Работа с актами передачи
/// </summary>
[ApiController]
[Route("[controller]")]
public class TransferActsController : ControllerBase
{
    private readonly ITransferActService transferActService;
    private readonly IMapper mapper;
    private readonly IValidateService validateService;
    private readonly ITransferActExcelService transferActExcelService;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера актов передачи
    /// </summary>
    public TransferActsController(ITransferActService transferActService, IMapper mapper,
        IValidateService validateService,
        ITransferActExcelService transferActExcelService)
    {
        this.transferActService = transferActService;
        this.mapper = mapper;
        this.validateService = validateService;
        this.transferActExcelService = transferActExcelService;
    }

    /// <summary>
    /// Получает список всех актов передачи
    /// </summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<TransferActApiModel>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var acts = await transferActService.GetTransferActs(cancellationToken);
        return Ok(mapper.Map<IReadOnlyCollection<TransferActApiModel>>(acts));
    }

    /// <summary>
    /// Получает акт передачи по идентификатору
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<TransferActApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var act = await transferActService.GetTransferActById(id, cancellationToken);
        return Ok(mapper.Map<TransferActApiModel>(act));
    }

    /// <summary>
    /// Создаёт новый акт передачи
    /// </summary>
    [HttpPost]
    [ProducesResponseType<TransferActApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] TransferActCreateApiModel createModel, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<TransferActCreateModel>(createModel);
        await validateService.ValidateAsync(mapped, cancellationToken);
        var result = await transferActService.CreateTransferAct(mapped, cancellationToken);
        return Ok(mapper.Map<TransferActApiModel>(result));
    }

    /// <summary>
    /// Обновляет данные акта передачи по идентификатору
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] TransferActCreateApiModel updateModel, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<TransferActModel>(updateModel);
        await validateService.ValidateAsync(mapped, cancellationToken);
        mapped.Id = id;
        await transferActService.UpdateTransferAct(mapped, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Удаляет акт передачи по идентификатору
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await transferActService.DeleteTransferAct(id, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// акт передачи в Excel
    /// </summary>
    [HttpGet("{id:guid}/export")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExportToExcel(Guid id, CancellationToken cancellationToken)
    {
        var act = await transferActService.GetTransferActById(id, cancellationToken);
        var excelBytes = await transferActExcelService.ExportTransferActAsync(act.Id, cancellationToken);

        return File(excelBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"Акт_передачи_{act.Number}.xlsx");
    }
}
