namespace OfficeProcurement.Common;

/// <summary>
/// Модель элемента валидации
/// </summary>
public class InvalidateItemModel
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="InvalidateItemModel"/>
    /// </summary>
    /// <param name="field">Имя невалидного поля</param>
    /// <param name="message">Сообщение валидации</param>
    public InvalidateItemModel(string field, string message)
    {
        Field = field;
        Message = message;
    }

    /// <summary>
    /// Имя невалидного поля
    /// </summary>
    /// <remarks>Если пустое значит валидация относится ко всей модели</remarks>
    public string Field { get; }

    /// <summary>
    /// сообщение валидации
    /// </summary>
    public string Message { get; }
}
