namespace OfficeProcurement.Services.Contracts.Models;

/// <summary>
/// Модель для создания акта передачи
/// </summary>
public class TransferActCreateModel
{
    public Guid ContractId { get; set; }

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
