namespace OfficeProcurement.Entities;

/// <summary>
/// Сущность содержимого акта
/// </summary>
public class TransferActItem
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
    /// Количество
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Цена за единицу товара
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Общая стоимость
    /// </summary>
    public decimal GetTotalAmount()=> Quantity * UnitPrice;

    /// <summary>
    /// НДС
    /// </summary>
    public decimal GetVatAmount(decimal vatRate = 0.20m) => GetTotalAmount() * vatRate;
}
