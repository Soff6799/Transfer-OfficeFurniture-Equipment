using Microsoft.EntityFrameworkCore;
using OfficeProcurement.Entities.Configurations;
using OfficeProcurement.Dal.Contracts.Repositories;

namespace OfficeProcurement.Context;

public class OfficeProcurementContext: DbContext
{
    /// <summary>
    /// Инициирует новый экземпляр <see cref="FinalExerciseContext"/>
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
    }
}
