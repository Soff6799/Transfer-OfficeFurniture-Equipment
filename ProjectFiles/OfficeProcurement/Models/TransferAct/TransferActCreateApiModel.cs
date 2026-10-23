namespace OfficeProcurement.Models.TransferAct;

/// <summary>
/// Модель создания акта передачи для API
/// </summary>
public class TransferActCreateApiModel
{
    public Guid ContractId { get; set; }

    /// <summary>
    /// Номер акта
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// Дата составления акта
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Город составления акта
    /// </summary>
    public string City { get; set; } = string.Empty;
}
