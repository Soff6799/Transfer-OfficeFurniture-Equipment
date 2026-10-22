namespace OfficeProcurement.Models.TransferActItem;

/// <summary>
/// Модель позиции акта передачи для API
/// </summary>
public class TransferActItemApiModel : TransferActItemCreateApiModel
{
    /// <summary>
    /// Идентификатор позиции
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор акта передачи
    /// </summary>
    public Guid TransferActId { get; set; }
}
