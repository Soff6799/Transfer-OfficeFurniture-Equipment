using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfficeProcurement.Models;
using OfficeProcurement.Common;
using OfficeProcurement.Models.Client;
using OfficeProcurement.Services.Contracts.Interface;
using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Api.Controllers;

/// <summary>
/// Работа с клиентами
/// </summary>
[ApiController]
[Route("[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService clientService;
    private readonly IMapper mapper;
    private readonly IValidateService validateService;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера клиентов
    /// </summary>
    /// <param name="clientService">Сервис для работы с клиентами</param>
    /// <param name="mapper">Маппер для преобразования моделей</param>
    /// <param name="validateService">Сервис валидации моделей</param>
    public ClientsController(IClientService clientService, IMapper mapper, IValidateService validateService)
    {
        this.clientService = clientService;
        this.mapper = mapper;
        this.validateService = validateService;
    }

    /// <summary>
    /// Получает список всех клиентов
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Коллекция клиентов</returns>
    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<ClientApiModel>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var clients = await clientService.GetClients(cancellationToken);
        return Ok(mapper.Map<IReadOnlyCollection<ClientApiModel>>(clients));
    }

    /// <summary>
    /// Получает клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Модель клиента</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<ClientApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var client = await clientService.GetClientById(id, cancellationToken);
        return Ok(mapper.Map<ClientApiModel>(client));
    }

    /// <summary>
    /// Создаёт нового клиента
    /// </summary>
    /// <param name="createModel">Данные создаваемого клиента</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Созданный клиент</returns>
    [HttpPost]
    [ProducesResponseType<ClientApiModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] ClientCreateApiModel createModel, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<ClientCreateModel>(createModel);
        await validateService.ValidateAsync(mapped, cancellationToken);
        var result = await clientService.CreateClient(mapped, cancellationToken);
        return Ok(mapper.Map<ClientApiModel>(result));
    }

    /// <summary>
    /// Обновляет данные клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    /// <param name="updateModel">Обновлённые данные клиента</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ClientCreateApiModel updateModel, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<ClientModel>(updateModel);
        await validateService.ValidateAsync(mapped, cancellationToken);
        mapped.Id = id;
        await clientService.UpdateClient(mapped, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Удаляет клиента по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await clientService.DeleteClient(id, cancellationToken);
        return NoContent();
    }
}
