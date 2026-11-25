using Microsoft.EntityFrameworkCore;
using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Entities;
using OfficeProcurement.Entities.Configurations;
using OfficeProcurement.Entities.Enums;
using OfficeProcurement.Infrastructure.Common;
using OfficeProcurement.Infrastructure.Providers;

namespace OfficeProcurement.Context;

/// <summary>
/// Контекст базы данных. Предоставляет доступ к наборам сущностей и реализует паттерны
/// </summary>
public class OfficeProcurementContext : DbContext,
    IReader,
    IWriter,
    IUnitOfWork,
    IDbWriterContext
{
    /// <summary>
    /// Инициирует новый экземпляр <see cref="OfficeProcurementContext"/>
    /// </summary>
    public OfficeProcurementContext(DbContextOptions<OfficeProcurementContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }

    /// <inheridoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IEntitiesAnchor).Assembly);

        // Сидинг тестовых пользователей
        var adminUser = new User
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Login = "admin",
            PasswordHash = HashPassword("admin123"),
            Role = UserRole.Admin,
            FullName = "Манжосова София Александровна",
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            CreatedBy = "System"
        };

        var managerUser = new User
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            Login = "manager",
            PasswordHash = HashPassword("manager123"),
            Role = UserRole.Manager,
            FullName = "Иванов Иван Иванович",
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            CreatedBy = "System"
        };

        modelBuilder.Entity<User>().HasData(adminUser, managerUser);
    }

    IQueryable<TEntity> IReader.Read<TEntity>()
        => base.Set<TEntity>()
            .AsNoTracking();

    void IWriter.Add<TEntity>(TEntity entity)
        => base.Entry(entity).State = EntityState.Added;

    void IWriter.Update<TEntity>(TEntity entity)
        => base.Entry(entity).State = EntityState.Modified;

    void IWriter.Delete<TEntity>(TEntity entity)
        => base.Set<TEntity>().Remove(entity);

    async Task<int> IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        var count = await base.SaveChangesAsync(cancellationToken);
        foreach (var entry in base.ChangeTracker.Entries().ToArray())
        {
            entry.State = EntityState.Detached;
        }

        return count;

    }

    public IWriter Writer => this;

    /// <inheritdoc/>
    public IDateTimeProvider DateTimeProvider { get; } = new DateTimeProvider();

    /// <inheritdoc/>
    public IIdentityProvider IdentityProvider { get; } = new IdentityProvider();

    /// <summary>
    /// Вычисляет SHA256-хеш пароля. Используется только для сидинга тестовых данных
    /// </summary>
    private static string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}

