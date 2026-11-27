using FluentValidation;
using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Services.Validators.TransferAct;

/// <summary>
/// Базовый валидатор моделей акта передачи
/// </summary>
public abstract class TransferActModelBaseValidator<T> : AbstractValidator<T>
    where T : TransferActCreateModel
{
    /// <summary>
    /// Инициализирует валидатор и настраивает правила
    /// </summary>
    protected TransferActModelBaseValidator()
    {
        RuleFor(x => x.ContractId)
            .NotEmpty().WithMessage("Идентификатор договора обязателен")
            .NotEqual(Guid.Empty).WithMessage("Идентификатор договора не может быть пустым");

        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Номер акта обязателен")
            .MaximumLength(50).WithMessage("Номер акта не должен превышать 50 символов");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Дата составления обязательна")
            .Must(d => d >= new DateOnly(1900, 1, 1)).WithMessage("Дата не может быть раньше 1900 года")
            .Must(d => d <= DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Дата не может быть в будущем");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("Город составления обязателен")
            .MaximumLength(50).WithMessage("Город не должен превышать 50 символов");
    }

}
