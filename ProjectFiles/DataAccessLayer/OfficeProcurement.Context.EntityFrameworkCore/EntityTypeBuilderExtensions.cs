using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfficeProcurement.Dal.Contracts;

namespace OfficeProcurement.Context.EntityFrameworkCore;

public static class EntityTypeBuilderExtensions
{
    /// <summary>
    /// Задаёт конфигурацию ключа для идентификатора <see cref="IEntityWithId"/>
    /// </summary>
    public static void HasIdAsKey<T>(this EntityTypeBuilder<T> builder)
        where T : class, IEntityWithId
        => builder.HasKey(x => x.Id);
}
