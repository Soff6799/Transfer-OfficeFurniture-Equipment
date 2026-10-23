using OfficeProcurement.Dal.Contracts;

namespace OfficeProcurement.Entities;

/// <summary>
/// Сущность товара
/// </summary>
public class Product : BaseAuditEntity
{
    /// <summary>
    /// Идентификатор поставщика
    /// </summary>
    public Guid SupplierId { get; set; }

    /// <summary>
    /// Навигационное свойство к поставщику
    /// </summary>
    public virtual Supplier? Supplier { get; set; }

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
    /// Цена
    /// </summary>
    public decimal Price { get; set; }

}
