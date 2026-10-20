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
        private readonly Dictionary<Type, IValidator> validators = new();

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="ValidateService"/>
        /// </summary>
        public ValidateService(IEnumerable<IValidator> validators)
        {
            foreach (var validator in validators)
            {
                var validatorType = validator.GetType();
                var modelType = validatorType.BaseType?.GenericTypeArguments.FirstOrDefault();

                if (modelType is null)
                {
                    throw new InvalidOperationException($"Был зарегистрирован неверный тип валидатора: {validator}");
                }

                this.validators[modelType] = validator;
            }
        }

        /// <inheredoc cref= "IValidateService.ValidateAsync{TModel}"/>
        public async Task ValidateAsync<TModel>(TModel model, CancellationToken cancellationToken)
        where TModel : class
        {
            var modelType = model.GetType();
            if (!validators.TryGetValue(modelType, out var validator))
            {
                throw new InvalidOperationException($"Не найден валидатор для {modelType}");
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
