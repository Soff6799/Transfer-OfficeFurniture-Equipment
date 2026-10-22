namespace OfficeProcurement.Models.Contract;

/// <summary>
/// Модель договора для API
/// </summary>
public class ContractApiModel : ContractCreateApiModel
{
    /// <summary>
    /// Идентификатор договора
    /// </summary>
    public Guid Id { get; set; }
}
