namespace OfficeProcurement.Services.Contracts.Models;

/// <summary>
/// Модель для создания позиции акта передачи
/// </summary>
public class TransferActItemCreateModel
{
    public Guid TransferActId { get; set; }
    public Guid ProductId { get; set; }

    /// <summary>
    /// Название позиции
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Складской номер
    /// </summary>
    public string? StockNumber { get; set; }

    /// <summary>
    /// Заводские характеристики
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
}
