namespace OfficeProcurement.Entities;

/// <summary>
/// Сущность товара
/// </summary>
public class Product
{
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Складской номер
    /// </summary>
    public string? StockNumber  { get; set; }

    /// <summary>
    /// Заводские характеристики (Цвет: серый, 100x50x70)
    /// </summary>
    public string? Specifications { get; set; }

    /// <summary>
    /// Единица измерения
    /// </summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// Цена
    /// </summary>
    public decimal Price { get; set; }

}
