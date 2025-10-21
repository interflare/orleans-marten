using Docker.DotNet;
using Docker.DotNet.Models;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

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
    private const string ServerImage = "postgres:16";
    private static readonly DockerClient Client = new DockerClientConfiguration().CreateClient();

    private readonly string _containerId;
    public readonly string ConnectionString;

    private PostgresDatabaseFixture(string containerId, string connectionString)
    {
        _containerId = containerId;
        ConnectionString = connectionString;
    }

    internal static async Task<PostgresDatabaseFixture> CreateAsync(string name, CancellationToken token = default)
    {
        await Client.Images.CreateImageAsync(new ImagesCreateParameters { FromImage = ServerImage }, null, new Progress<JSONMessage>(), token);

        var password = Guid.NewGuid().ToString();
        var createResponse = await Client.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Image = ServerImage,
                Name = name,
                HostConfig = new HostConfig
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        ["5432/tcp"] = [new PortBinding { HostPort = "0" }] // Random host port
                    }
                },
                Env = ["POSTGRES_DB=postgres", "POSTGRES_USER=postgres", $"POSTGRES_PASSWORD={password}"]
            }, token);

        var containerId = createResponse.ID;
        try
        {
            if (!await Client.Containers.StartContainerAsync(containerId, null, token))
                throw new InvalidOperationException("could not start container");

            var inspect = await Client.Containers.InspectContainerAsync(containerId, token);

            var dbPort = inspect.NetworkSettings.Ports["5432/tcp"].FirstOrDefault()?.HostPort;
            if (dbPort is null)
                throw new InvalidOperationException("could not determine host port");

            var connectionParams = new DbConnectionStringBuilder
            {
                ["Server"] = "127.0.0.1",
                ["Port"] = dbPort,
                ["Database"] = "postgres",
                ["User Id"] = "postgres",
                ["Password"] = password
            };

            return new PostgresDatabaseFixture(containerId, connectionParams.ToString());
        }
        catch
        {
            await DeleteAsync(containerId, token);
            throw;
        }
    }

    private static async Task DeleteAsync(string containerId, CancellationToken token = default)
        => await Client.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters { Force = true }, token);

    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => DeleteAsync(_containerId);
}