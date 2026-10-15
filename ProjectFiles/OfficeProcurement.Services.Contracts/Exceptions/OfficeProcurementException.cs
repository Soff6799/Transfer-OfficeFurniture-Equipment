namespace OfficeProcurement.Services.Contracts.Exceptions;

/// <summary>
/// Базовое исключение приложения
/// </summary>
public class OfficeProcurementException : Exception
{
    /// <summary>
    /// Инициализирует новый экземпляр исключения
    /// </summary>
    public OfficeProcurementException()
    {

    }

    /// <summary>
    /// Инициализирует новый экземпляр исключения с сообщением
    /// </summary>
    /// <param name="message">Сообщение об ошибке</param>
    public OfficeProcurementException(string message) : base(message)
    {

    }
}
