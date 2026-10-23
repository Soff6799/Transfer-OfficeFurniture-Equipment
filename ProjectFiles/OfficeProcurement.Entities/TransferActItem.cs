using OfficeProcurement.Dal.Contracts;

namespace OfficeProcurement.Entities;

/// <summary>
/// Сущность содержимого акта (Строка)
/// </summary>
public class TransferActItem : BaseAuditEntity
{
    /// <summary>
    /// Идентификатор акта передачи
    /// </summary>
    public Guid TransferActId { get; set; }

    /// <summary>
    /// Навигационное свойство к акту передачи
    /// </summary>
    public virtual TransferAct? TransferAct { get; set; }

    /// <summary>
    /// Идентификатор продукта
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Навигационное свойство к продукту
    /// </summary>
    public virtual Product? Product { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Складской номер
    /// </summary>
    public string? StockNumber { get; set; }

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
    public decimal GetTotalAmount() => Quantity * UnitPrice;

    /// <summary>
    /// НДС
    /// </summary>
    public decimal GetVatAmount(decimal vatRate = 0.20m) => GetTotalAmount() * vatRate;
}
