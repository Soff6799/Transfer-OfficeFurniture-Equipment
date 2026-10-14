namespace OfficeProcurement.Services.Contracts.Exceptions;

/// <summary>
/// Исключение, выбрасываемое при отсутствии сущности с указанным идентификатором
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
/// <param name="id">Идентификатор сущности</param>
public class EntityNotFoundException<T>(Guid id) : NotFoundException($"Сущность " +
    $"'{typeof(T).Name}' с идентификатором '{id}' не найдена");
