using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfficeProcurement.Common;
using OfficeProcurement.Models.Contract;
using OfficeProcurement.Services.Contracts.Interface;
using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Controllers;

/// <summary>
/// Работа с договорами
/// </summary>
[ApiController]
[Route("[controller]")]
public class ContractsController : ControllerBase
{
    private readonly IContractService contractService;
    private readonly IMapper mapper;
    private readonly IValidateService validateService;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера договоров
    /// </summary>
    public ContractsController(IContractService contractService, IMapper mapper, IValidateService validateService)
    {
        this.contractService = contractService;
        this.mapper = mapper;
        this.validateService = validateService;
    }

    /// <summary>
    /// Получает список всех договоров
    /// </summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<ContractApiModel>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var contracts = await contractService.GetContracts(cancellationToken);
        return Ok(mapper.Map<IReadOnlyCollection<ContractApiModel>>(contracts));
    }

    /// <summary>
    /// Получает договор по идентификатору
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<ContractApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var contract = await contractService.GetContractById(id, cancellationToken);
        return Ok(mapper.Map<ContractApiModel>(contract));
    }

    /// <summary>
    /// Создаёт новый договор
    /// </summary>
    [HttpPost]
    [ProducesResponseType<ContractApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] ContractCreateApiModel createModel, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<ContractCreateModel>(createModel);
        await validateService.ValidateAsync(mapped, cancellationToken);
        var result = await contractService.CreateContract(mapped, cancellationToken);
        return Ok(mapper.Map<ContractApiModel>(result));
    }

    /// <summary>
    /// Обновляет данные договора по идентификатору
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ContractCreateApiModel updateModel, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<ContractModel>(updateModel);
        await validateService.ValidateAsync(mapped, cancellationToken);
        mapped.Id = id;
        await contractService.UpdateContract(mapped, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Удаляет договор по идентификатору
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await contractService.DeleteContract(id, cancellationToken);
        return NoContent();
    }
}
