namespace OfficeProcurement.Entities;

public class TransferAct
{
    /// <summary>
    /// Номер акта
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// Дата составления
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Город составления
    /// </summary>
    public string City { get; set; } = string.Empty;
}
