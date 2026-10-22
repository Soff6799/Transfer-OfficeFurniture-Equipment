namespace APIProject.Models
{
    /// <summary>
    /// Модель ответа с ошибкой
    /// </summary>
    public class ApiExceptionDetail
    {
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Massage {  get; set; } = string.Empty;
    }
}
