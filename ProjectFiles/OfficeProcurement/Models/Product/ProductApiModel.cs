namespace OfficeProcurement.Models.Product;

/// <summary>
/// Модель продукта для API
/// </summary>
public class ProductApiModel : ProductCreateApiModel
{
    /// <summary>
    /// Идентификатор продукта
    /// </summary>
    public Guid Id { get; set; }

}
