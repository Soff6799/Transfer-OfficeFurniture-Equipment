using OfficeProcurement.Common;
namespace OfficeProcurement.Models
{
    /// <summary>
    /// модель ответа с ошибками валидации
    /// </summary>
    public class ApiValidationExceptionDetail
    {
        /// <summary>
        /// Ошибки валидации
        /// </summary>
        public IEnumerable<InvalidateItemModel> Errors { get; set; } = Array.Empty<InvalidateItemModel>();
    }
}
