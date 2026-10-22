namespace OfficeProcurement.Common
{
    /// <summary>
    /// сервис валидации
    /// </summary>
    public interface IValidateService
    {
        /// <summary>
        /// Валидация мрдели
        /// </summary>
        Task ValidateAsync<TModel>(TModel model, CancellationToken cancellationToken)
            where TModel : class;
    }
}
