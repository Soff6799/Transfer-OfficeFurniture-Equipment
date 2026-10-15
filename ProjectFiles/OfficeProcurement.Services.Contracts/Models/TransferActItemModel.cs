namespace OfficeProcurement.Services.Contracts.Models;

/// <summary>
/// Модель позиции акта передачи для чтения
/// </summary>
public class TransferActItemModel : TransferActItemCreateModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
}
