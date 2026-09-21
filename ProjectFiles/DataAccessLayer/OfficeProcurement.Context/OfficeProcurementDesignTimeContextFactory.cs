using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OfficeProcurement.Context;

/// <summary>
/// Фабрика для создания экземпляра <see cref="OfficeProcurementContext"/>
/// Используется инструментами EF Core для выполнения миграций и обновления базы данных
/// </summary>
public class OfficeProcurementDesignTimeContextFactory : IDesignTimeDbContextFactory<OfficeProcurementContext>
{
    /// <summary>
    /// Creates a new instance of a derived context
    /// </summary>
    /// <remarks>
    /// 1) dotnet tool install --global dotnet-ef
    /// 2) dotnet tool updat --global dotnet-ef
    /// 3) dotnet ef migrations add [name] --project DataAccessLayer/OfficeProcurement.Context/OfficeProcurement.Context.csproj
    /// 4) dotnet ef database update --project DataAccessLayer/OfficeProcurement.Context/OfficeProcurement.Context.csproj
    /// 5) dotnet ef database update [targetMigrationName] --project DataAccessLayer/OfficeProcurement.Context/OfficeProcurement.Context.csproj
    /// </remarks>

    /// <summary>
    /// Создаёт новый экземпляр <see cref="OfficeProcurementContext"/>
    /// для использования инструментами EF Core во время разработки
    /// </summary>
    public OfficeProcurementContext CreateDbContext(string[] args)
    {
        var connectionString = "Host=localhost;Port=5432;Database=OfficeProcurement;Username=postgres;Password=Qwerty54321";
        var options = new DbContextOptionsBuilder<OfficeProcurementContext>()
            .UseNpgsql(connectionString)
            .LogTo(Console.WriteLine)
            .Options;

        return new OfficeProcurementContext(options);

    }

}
