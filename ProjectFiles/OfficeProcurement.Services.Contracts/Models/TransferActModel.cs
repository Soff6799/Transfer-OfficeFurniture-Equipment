namespace OfficeProcurement.Services.Contracts.Models;

/// <summary>
/// Модель акта передачи для чтения
/// </summary>
public class TransferActModel : TransferActCreateModel
{
    /// <summary>
    /// Идентификатор акта
    /// </summary>
    public Guid Id { get; set; }
}
