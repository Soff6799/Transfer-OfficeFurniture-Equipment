namespace OfficeProcurement.Services.Contracts.Exceptions;

/// <summary>
/// Исключение конфликта состояния ресурса (HTTP 409 Conflict)
/// </summary>
public class OfficeProcurementConflictException : OfficeProcurementException
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="OfficeProcurementConflictException"/>
    /// </summary>
    public OfficeProcurementConflictException(string message)
        : base(message)
    {
    }
}
