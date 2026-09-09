namespace OfficeProcurement.Dal.Contracts.Repositories;

/// <summary>
/// Интерфейс паттерна Unit of Work для управления транзакциями
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Асинхронно сохраняет все изменения в базу данных
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
