namespace OfficeProcurement.Services.Contracts.Exceptions;

/// <summary>
/// Исключение, выбрасываемое при отсутствии запрашиваемой сущности
/// </summary>
/// <param name="message">Сообщение об ошибке</param>
public class NotFoundException(string message) : OfficeProcurementException(message);
