using Orleans.Providers.Marten.Integration.Tests.TestHelpers;
using System.Diagnostics.CodeAnalysis;

namespace Orleans.Providers.Marten.Integration.Tests.Infrastructure.Fixtures;

[ExcludeFromCodeCoverage]
public abstract class PostgresBackedSiloHostFixture : InMemorySiloHostFixture
{
    protected PostgresDatabaseFixture DatabaseFixture { get; private set; } = null!;

    public override async Task InitializeAsync()
    {
        DatabaseFixture = await PostgresDatabaseFixture.CreateAsync(name: $"marten-integration-test-db--{Guid.NewGuid()}");
        await DatabaseFixture.WaitUntilCanConnect();
        await base.InitializeAsync();
    }

    public override async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await DatabaseFixture.DisposeAsync();
    }
}