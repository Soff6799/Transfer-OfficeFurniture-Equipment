using OfficeProcurement.Infrastructure.Common;

namespace OfficeProcurement.Context.Tests;

/// <summary>
/// Тестовая заглушка провайдера идентичности для InMemory-тестов.
/// </summary>
public class TestIdentityProvider : IIdentityProvider
{
    /// <inheritdoc/>
    public string Name => "TestUser";
}
