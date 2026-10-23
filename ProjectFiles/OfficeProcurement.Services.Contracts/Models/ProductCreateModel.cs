namespace OfficeProcurement.Services.Contracts.Models;

/// <summary>
/// Модель для создания продукта
/// </summary>
public class ProductCreateModel
{
    public Guid SupplierId { get; set; }

    /// <summary>
    /// Название продукта
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// складской номер
    /// </summary>
    public string? StockNumber { get; set; }

    /// <summary>
    /// заводские характеристики
    /// </summary>
    public string? Specifications { get; set; }

    /// <summary>
    /// единица измерения
    /// </summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// Цена
    /// </summary>
    public decimal UnitPrice { get; set; }
}
