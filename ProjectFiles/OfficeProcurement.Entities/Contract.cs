using OfficeProcurement.Dal.Contracts;

namespace OfficeProcurement.Entities;

/// <summary>
/// Сущность Договора
/// </summary>
public class Contract : BaseAuditEntity
{
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public Guid ClientId { get; set; }

    /// <summary>
    /// Навигационное свойство к клиенту
    /// </summary>
    public virtual Client? Client { get; set; }

    /// <summary>
    /// Идентификатор поставщика
    /// </summary>
    public Guid SupplierId { get; set; }

    /// <summary>
    /// Навигационное свойство к поставщику
    /// </summary>
    public virtual Supplier? Supplier { get; set; }

    /// <summary>
    /// Номер договора
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// Дата подписания
    /// </summary>
    public DateTimeOffset SignedAt { get; set; }

    /// <summary>
    /// тип текущего договора
    /// </summary>
    public ContractType Type { get; set; }
}

/// <summary>
/// Перечисление поддерживаемых видов договоров (аренда/поставка)
/// </summary>
public enum ContractType
{
    Rent,
    Supply
}
