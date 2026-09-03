using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OfficeProcurement.Context;


public class OfficeProcurementDesignTimeContextFactory : IDesignTimeDbContextFactory<OfficeProcurementContext>
{
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
