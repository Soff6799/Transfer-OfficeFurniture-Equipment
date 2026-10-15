namespace OfficeProcurement.Services.Contracts.Models;

/// <summary>
/// Модель клиента для чтения
/// </summary>
public class ClientModel : ClientCreateModel
{
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public Guid Id { get; set; }
}
