namespace OfficeProcurement.Models.TransferAct;

/// <summary>
/// Модель акта передачи для API
/// </summary>
public class TransferActApiModel : TransferActCreateApiModel
{
    /// <summary>
    /// Идентификатор акта
    /// </summary>
    public Guid Id { get; set; }
}
