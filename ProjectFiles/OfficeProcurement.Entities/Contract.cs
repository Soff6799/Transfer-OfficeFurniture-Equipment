namespace OfficeProcurement.Entities;

/// <summary>
/// Сущность Договора
/// </summary>
public class Contract
{
    /// <summary>
    /// Номер договора
    /// </summary>
    public string Number { get; set; } =  string.Empty;

    /// <summary>
    /// Дата подписания
    /// </summary>
    public DateTimeOffset SignedAt  { get; set; }

    /// <summary>
    /// тип текущего договора
    /// </summary>
    public ContractType Type  { get; set; }
}

/// <summary>
/// Перечисление поддерживаемых видов договоров (аренда/поставка)
/// </summary>
public enum ContractType
{
    Rent,
    Supply
}
