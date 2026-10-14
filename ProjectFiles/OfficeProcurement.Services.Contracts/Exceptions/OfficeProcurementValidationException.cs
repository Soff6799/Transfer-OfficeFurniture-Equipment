using OfficeProcurement.Common;

namespace OfficeProcurement.Services.Contracts.Exceptions;
/// <summary>
/// Исключение, выбрасываемое при ошибках валидации модели
/// </summary>
public class FinalExerciseValidationException : OfficeProcurementException
{
    /// <summary>
    /// Ошибки
    /// </summary>
    public IEnumerable<InvalidateItemModel> Errors { get; }

    /// <summary>
    /// Инициализирует новый экземпляр исключения со списком ошибок валидации
    /// </summary>
    /// <param name="errors">Список ошибок валидации</param>
    public FinalExerciseValidationException(IEnumerable<InvalidateItemModel> errors)
    {
        Errors = errors;
    }
}
