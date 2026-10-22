namespace OfficeProcurement.Models.Supplier;

/// <summary>
/// Модель поставщика для API
/// </summary>
public class SupplierApiModel : SupplierCreateApiModel
{
    /// <summary>
    /// Идентификатор поставщика
    /// </summary>
    public Guid Id { get; set; }
}
