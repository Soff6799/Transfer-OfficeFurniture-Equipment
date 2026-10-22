using FluentValidation;
using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Services.Validators.Contract;

/// <summary>
/// Базовый валидатор моделей договора
/// </summary>
public abstract class ContractModelBaseValidator<T> : AbstractValidator<T>
    where T : ContractCreateModel
{
    /// <summary>
    /// Инициализирует валидатор и настраивает правила
    /// </summary>
    protected ContractModelBaseValidator()
    {
        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Номер договора обязателен")
            .MaximumLength(50).WithMessage("Номер договора не должен превышать 50 символов");

        RuleFor(x => x.SignedAt)
            .NotEmpty().WithMessage("Дата подписания обязательна");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Недопустимый тип договора");
    }
}
