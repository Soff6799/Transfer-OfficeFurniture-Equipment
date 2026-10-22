namespace OfficeProcurement.Models
{
    /// <summary>
    /// Модель ответа с ошибкой
    /// </summary>
    public class ApiExceptionDetail
    {
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
