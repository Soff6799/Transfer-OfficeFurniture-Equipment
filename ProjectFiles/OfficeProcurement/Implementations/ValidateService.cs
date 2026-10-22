using OfficeProcurement.Common;
using FluentValidation;
using OfficeProcurement.Services.Contracts.Exceptions;

namespace OfficeProcurement.Implementations.ValidateService
{
    /// <summary>
    /// сервис валидации
    /// </summary>
    public class ValidateService : IValidateService
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="ValidateService"/>
        /// </summary>
        /// <param name="serviceProvider">Провайдер сервисов</param>
        public ValidateService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <inheritdoc cref="IValidateService.ValidateAsync{TModel}"/>
        public async Task ValidateAsync<TModel>(TModel model, CancellationToken cancellationToken)
            where TModel : class
        {
            var validator = serviceProvider.GetService(typeof(IValidator<TModel>)) as IValidator<TModel>;
            if (validator == null)
            {
                throw new InvalidOperationException($"Не найден валидатор для {typeof(TModel)}");
            }

            var context = new ValidationContext<TModel>(model);
            var validationResult = await validator.ValidateAsync(context, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => new InvalidateItemModel(x.PropertyName, x.ErrorMessage));
                throw new OfficeProcurementValidationException(errors);
            }
        }
    }
}
