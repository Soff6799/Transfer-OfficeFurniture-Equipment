using OfficeProcurement.Services.Contracts.Models;

namespace OfficeProcurement.Services.Contracts.Interface;

/// <summary>
/// Сервис экспорта акта передачи в Excel
/// </summary>
public interface ITransferActExcelService
{
    /// <summary>
    /// Экспортирует акт передачи в формате Excel
    /// </summary>
    Task<byte[]> ExportTransferActAsync(Guid transferActId, CancellationToken cancellationToken);
}
