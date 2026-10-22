using FluentValidation;
using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Services.Validators.Supplier;

/// <summary>
/// Базовый валидатор моделей поставщика
/// </summary>
public abstract class SupplierModelBaseValidator<T> : AbstractValidator<T>
    where T : SupplierCreateModel
{
    /// <summary>
    /// Инициализирует валидатор и настраивает правила для полей поставщика
    /// </summary>
    protected SupplierModelBaseValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Название компании обязательно")
            .MaximumLength(200).WithMessage("Название компании не должно превышать 200 символов");

        RuleFor(x => x.ContactLastName)
            .NotEmpty().WithMessage("Фамилия контактного лица обязательна")
            .MaximumLength(200).WithMessage("Фамилия не должна превышать 200 символов");

        RuleFor(x => x.ContactName)
            .NotEmpty().WithMessage("Имя контактного лица обязательно")
            .MaximumLength(200).WithMessage("Имя не должно превышать 200 символов");

        RuleFor(x => x.ContactPatronymic)
            .MaximumLength(200).WithMessage("Отчество не должно превышать 200 символов");

        RuleFor(x => x.CeoBasis)
            .NotEmpty().WithMessage("Основания действий обязательны")
            .MaximumLength(200).WithMessage("Основания действий не должны превышать 200 символов");

        RuleFor(x => x.Inn)
            .NotEmpty().WithMessage("ИНН обязателен")
            .Matches(@"^(\d{10}|\d{12})$").WithMessage("ИНН должен состоять из 10 или 12 цифр")
            .MaximumLength(12).WithMessage("ИНН не должен превышать 12 символов");

        RuleFor(x => x.LegalAddress)
            .NotEmpty().WithMessage("Юридический адрес обязателен")
            .MaximumLength(400).WithMessage("Адрес не должен превышать 400 символов");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email обязателен")
            .EmailAddress().WithMessage("Некорректный формат email")
            .MaximumLength(100).WithMessage("Email не должен превышать 100 символов");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Телефон обязателен")
            .MaximumLength(20).WithMessage("Телефон не должен превышать 20 символов");
    }
}
