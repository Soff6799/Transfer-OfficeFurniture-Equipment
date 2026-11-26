using OfficeProcurement.Common;

namespace OfficeProcurement.Services.Contracts.Exceptions;
/// <summary>
/// Исключение, выбрасываемое при ошибках валидации модели
/// </summary>
public class OfficeProcurementValidationException : OfficeProcurementException
{
    /// <summary>
    /// Ошибки валидации
    /// </summary>
    public IReadOnlyCollection<InvalidateItemModel> Errors { get; }

    /// <summary>
    /// Инициализирует новый экземпляр исключения со списком ошибок валидации
    /// </summary>
    /// <param name="errors">Список ошибок валидации</param>
    public OfficeProcurementValidationException(IEnumerable<InvalidateItemModel> errors)
        : this(errors.ToList())
    {
    }
    private OfficeProcurementValidationException(IReadOnlyCollection<InvalidateItemModel> errors)
        : base($"Валидация не пройдена: {string.Join("; ", errors.Select(e => $"{e.Field}: {e.Message}"))}")
    {
        Errors = errors;
    }
}
