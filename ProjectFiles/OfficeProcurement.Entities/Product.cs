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
    /// Описание состояния/характеристик
    /// </summary>
    public string? Condition { get; set; }

    /// <summary>
    /// Единица измерения
    /// </summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// Количество
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Цена за единицу
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
