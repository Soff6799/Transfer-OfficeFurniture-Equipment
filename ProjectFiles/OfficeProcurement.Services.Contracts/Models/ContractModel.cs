namespace OfficeProcurement.Services.Contracts.Models;

/// <summary>
/// Модель договора для чтения
/// </summary>
public class ContractModel : ContractCreateModel
{
    /// <summary>
    /// Идентификатор договора
    /// </summary>
    public Guid Id { get; set; }
}
