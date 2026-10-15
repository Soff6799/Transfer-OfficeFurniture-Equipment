namespace OfficeProcurement.Services.Contracts.Models;

/// <summary>
/// Модель продукта для чтения
/// </summary>
public class ProductModel : ProductCreateModel
{
    /// <summary>
    /// Идентификатор продукта
    /// </summary>
    public Guid Id { get; set; }
}
