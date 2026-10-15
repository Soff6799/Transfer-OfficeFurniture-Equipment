namespace OfficeProcurement.Services.Contracts.Models;

/// <summary>
/// Модель поставщика для чтения
/// </summary>
public class SupplierModel : SupplierCreateModel
{
    /// <summary>
    /// Идентификатор поставщика
    /// </summary>
    public Guid Id { get; set; }
}
