using OfficeProcurement.Common;

namespace OfficeProcurement.Services.Contracts.Exceptions;
/// <summary>
/// Исключение, выбрасываемое при ошибках валидации модели
/// </summary>
public class OfficeProcurementValidationException : OfficeProcurementException
{
    /// <summary>
    /// Ошибки
    /// </summary>
    public IEnumerable<InvalidateItemModel> Errors { get; }

    /// <summary>
    /// Инициализирует новый экземпляр исключения со списком ошибок валидации
    /// </summary>
    /// <param name="errors">Список ошибок валидации</param>
    public OfficeProcurementValidationException(IEnumerable<InvalidateItemModel> errors)
    {
        Errors = errors;
    }
}
