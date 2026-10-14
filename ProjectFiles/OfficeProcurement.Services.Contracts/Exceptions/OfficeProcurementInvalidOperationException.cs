namespace OfficeProcurement.Services.Contracts.Exceptions;

/// <summary>
/// Исключение, выбрасываемое при недопустимой операции
/// </summary>
/// <param name="message">Сообщение об ошибке</param>
public class OfficeProcurementInvalidOperationException(string message) : OfficeProcurementException(message);

