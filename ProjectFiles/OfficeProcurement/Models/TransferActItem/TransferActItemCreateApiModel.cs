namespace OfficeProcurement.Models.TransferActItem;

/// <summary>
/// Модель создания позиции акта передачи для API
/// </summary>
public class TransferActItemCreateApiModel
{
    /// <summary>
    /// Название позиции
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Складской номер
    /// </summary>
    public string? StockNumber { get; set; }

    /// <summary>
    /// Характеристики позиции
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
    /// Цена за единицу
    /// </summary>
    public decimal UnitPrice { get; set; }
}
