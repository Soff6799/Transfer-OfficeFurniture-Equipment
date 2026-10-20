using FluentValidation;
using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Services.Validators.Product;

/// <summary>
/// Базовый валидатор моделей продукта
/// </summary>
/// <typeparam name="T">Тип проверяемой модели</typeparam>
public abstract class ProductModelBaseValidator<T> : AbstractValidator<T>
    where T : ProductCreateModel
{
    /// <summary>
    /// Инициализирует валидатор и настраивает правила для полей продукта
    /// </summary>
    protected ProductModelBaseValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название продукта обязательно")
            .MaximumLength(100).WithMessage("Название продукта не должно превышать 100 символов");

        RuleFor(x => x.StockNumber)
            .MaximumLength(100).WithMessage("Складской номер не должен превышать 100 символов");

        RuleFor(x => x.Specifications)
            .MaximumLength(300).WithMessage("Характеристики не должны превышать 300 символов");

        RuleFor(x => x.Unit)
            .NotEmpty().WithMessage("Единица измерения обязательна")
            .MaximumLength(20).WithMessage("Единица измерения не должна превышать 20 символов");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Цена должна быть больше нуля")
            .Must(price => decimal.Round(price, 2) == price)
            .WithMessage("Цена не может иметь больше 2 знаков после запятой");
    }
}
