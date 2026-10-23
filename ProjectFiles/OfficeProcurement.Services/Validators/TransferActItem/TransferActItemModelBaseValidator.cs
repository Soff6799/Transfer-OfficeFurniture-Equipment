using FluentValidation;
using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Services.Validators.TransferActItem;

/// <summary>
/// Базовый валидатор моделей позиции акта передачи
/// </summary>
public abstract class TransferActItemModelBaseValidator<T> : AbstractValidator<T>
    where T : TransferActItemCreateModel
{
    /// <summary>
    /// Инициализирует валидатор и настраивает правила для полей позиции акта передачи
    /// </summary>
    protected TransferActItemModelBaseValidator()
    {
        RuleFor(x => x.TransferActId)
            .NotEmpty().WithMessage("Идентификатор акта передачи обязателен")
            .NotEqual(Guid.Empty).WithMessage("Идентификатор акта передачи не может быть пустым");

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Идентификатор продукта обязателен")
            .NotEqual(Guid.Empty).WithMessage("Идентификатор продукта не может быть пустым");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название позиции обязательно")
            .MaximumLength(100).WithMessage("Название позиции не должно превышать 100 символов");

        RuleFor(x => x.StockNumber)
            .MaximumLength(100).WithMessage("Складской номер не должен превышать 100 символов");

        RuleFor(x => x.Specifications)
            .MaximumLength(300).WithMessage("Характеристики не должны превышать 300 символов");

        RuleFor(x => x.Unit)
            .NotEmpty().WithMessage("Единица измерения обязательна")
            .MaximumLength(20).WithMessage("Единица измерения не должна превышать 20 символов");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Количество должно быть больше нуля")
            .Must(q => decimal.Round(q, 3) == q)
            .WithMessage("Количество не может иметь больше 3 знаков после запятой");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("Цена за единицу должна быть больше нуля")
            .Must(p => decimal.Round(p, 2) == p)
            .WithMessage("Цена не может иметь больше 2 знаков после запятой");
    }

}
