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
    private readonly PostgreSqlContainer _postgresContainer;
    public readonly string ConnectionString;

    private PostgresDatabaseFixture(PostgreSqlContainer postgresContainer)
    {
        _postgresContainer = postgresContainer;
        ConnectionString = postgresContainer.GetConnectionString();
    }

    internal static async Task<PostgresDatabaseFixture> CreateAsync(string name, CancellationToken token = default)
    {
        var postgresContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16")
            .WithName(name)
            .WithPortBinding(5432, true)
            .Build();

        await postgresContainer.StartAsync(token);

        return new PostgresDatabaseFixture(postgresContainer);
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => _postgresContainer.DisposeAsync().AsTask();
}