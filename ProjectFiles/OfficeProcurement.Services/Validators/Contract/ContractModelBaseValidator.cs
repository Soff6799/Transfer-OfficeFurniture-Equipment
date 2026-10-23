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
        RuleFor(x => x.ClientId)
            .NotEmpty().WithMessage("Идентификатор клиента обязателен")
            .NotEqual(Guid.Empty).WithMessage("Идентификатор клиента не может быть пустым");

        RuleFor(x => x.SupplierId)
            .NotEmpty().WithMessage("Идентификатор поставщика обязателен")
            .NotEqual(Guid.Empty).WithMessage("Идентификатор поставщика не может быть пустым");

        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Номер договора обязателен")
            .MaximumLength(50).WithMessage("Номер договора не должен превышать 50 символов");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Дата подписания обязательна");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Недопустимый тип договора");
    }
}
