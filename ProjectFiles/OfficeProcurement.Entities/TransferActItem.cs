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
    public decimal Quantity { get; set; }

    /// <summary>
    /// Цена за единицу товара
    /// </summary>
    public decimal Price { get; set; }

}
