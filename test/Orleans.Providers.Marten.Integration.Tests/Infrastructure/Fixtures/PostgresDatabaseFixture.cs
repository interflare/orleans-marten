using System.Diagnostics.CodeAnalysis;
using Testcontainers.PostgreSql;

namespace Orleans.Providers.Marten.Integration.Tests.Infrastructure.Fixtures;

/// <summary>
/// Implements a fixture for a Postgres database.
/// </summary>
/// <remarks>
/// This fixture creates a Postgres database using a Docker container, meaning that Podman or Docker must be installed on the host machine.
/// </remarks>
[ExcludeFromCodeCoverage]
public class PostgresDatabaseFixture : IAsyncLifetime
{
    private PostgreSqlContainer? _postgresContainer;
    public string ConnectionString { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        var name = $"marten-integration-test-db--{Guid.NewGuid()}";
        _postgresContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16")
            .WithName(name)
            .WithPortBinding(5432, true)
            .Build();

        await _postgresContainer.StartAsync();
        ConnectionString = _postgresContainer.GetConnectionString();
    }

    public async Task DisposeAsync()
    {
        if (_postgresContainer != null) await _postgresContainer.DisposeAsync();
    }
}