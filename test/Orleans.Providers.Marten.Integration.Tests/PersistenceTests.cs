using JasperFx;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using Orleans.Providers.Marten.Integration.Tests.Infrastructure.Fixtures;
using Orleans.Providers.Marten.Integration.Tests.TestGrains;
using Orleans.Providers.Marten.Integration.Tests.TestHelpers;
using Orleans.Providers.Marten.Persistence.Documents;
using Orleans.Storage;
using System.Text.Json;

namespace Orleans.Providers.Marten.Integration.Tests;

public class PersistenceTests : IClassFixture<PostgresDatabaseFixture>, IAsyncLifetime
{
    private readonly PostgresDatabaseFixture _databaseFixture;
    private IHost _host = null!;

    public PersistenceTests(PostgresDatabaseFixture databaseFixture)
    {
        _databaseFixture = databaseFixture;
    }

    private IGrainFactory GrainFactory => _host.Services.GetRequiredService<IGrainFactory>();
    private IDocumentStore DocumentStore => _host.Services.GetRequiredService<IDocumentStore>();

    public async Task InitializeAsync()
    {
        await _databaseFixture.WaitUntilCanConnect();

        var hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddMarten(options => options.Connection(_databaseFixture.ConnectionString))
                    .UseLightweightSessions();
            })
            .UseOrleans(siloBuilder => siloBuilder
                .UseLocalhostClustering()
                .AddMartenGrainStorageAsDefault());

        _host = hostBuilder.Build();
        await _host.StartAsync();
    }

    public async Task DisposeAsync() => await _host.StopAsync();

    [Fact]
    public async Task StatePersistenceReadWrite()
    {
        // Arrange
        var shortUrl = Guid.NewGuid().GetHashCode().ToString("x");
        var grain = GrainFactory.GetGrain<IUrlShortenerGrain>(shortUrl);

        await using var documentSession = DocumentStore.LightweightSession();

        // Act
        await grain.SetUrl("https://example.com");

        // Assert
        var document = await documentSession.LoadAsync<OrleansState>($"{ClusterOptions.DefaultServiceId}-url-{grain.GetGrainId()}");

        Assert.NotNull(document);
        Assert.NotNull(document.Data);
        Assert.Equal($"{ClusterOptions.DefaultServiceId}-url-{grain.GetGrainId()}", document.Id);
        Assert.Equal("url", document.StateName);
        Assert.Equal(grain.GetGrainId().ToString(), document.GrainId);

        var state = JsonSerializer.Deserialize<UrlDetails>(document.Data);
        Assert.NotNull(state);
        Assert.Equal("https://example.com", state.FullUrl);
        Assert.Equal(shortUrl, state.ShortenedRouteSegment);
    }

    [Fact]
    public async Task StatePersistenceDeletion()
    {
        // Arrange
        var shortUrl = Guid.NewGuid().GetHashCode().ToString("x");
        var grain = GrainFactory.GetGrain<IUrlShortenerGrain>(shortUrl);

        await using var documentSession = DocumentStore.LightweightSession();

        // Act & Assert
        await grain.SetUrl("https://example.com");

        var document = await documentSession.LoadAsync<OrleansState>($"{ClusterOptions.DefaultServiceId}-url-{grain.GetGrainId()}");
        Assert.NotNull(document);

        await grain.Delete();

        document = await documentSession.LoadAsync<OrleansState>($"{ClusterOptions.DefaultServiceId}-url-{grain.GetGrainId()}");
        Assert.Null(document);
    }

    [Fact]
    public async Task StateConcurrencyWrite()
    {
        // Arrange
        var shortUrl = Guid.NewGuid().GetHashCode().ToString("x");
        var grain = GrainFactory.GetGrain<IUrlShortenerGrain>(shortUrl);

        await using var documentSession = DocumentStore.LightweightSession();

        // Act & Assert
        await grain.SetUrl("https://example.com");

        var document = await documentSession.LoadAsync<OrleansState>($"{ClusterOptions.DefaultServiceId}-url-{grain.GetGrainId()}");
        Assert.NotNull(document);

        document = document with { Data = string.Empty };
        documentSession.Update(document);
        await documentSession.SaveChangesAsync();

        await Assert.ThrowsAsync<InconsistentStateException>(() => grain.SetUrl("https://example.net"));
    }

    [Fact]
    public async Task StateConcurrencyDeletion()
    {
        // Arrange
        var shortUrl = Guid.NewGuid().GetHashCode().ToString("x");
        var grain = GrainFactory.GetGrain<IUrlShortenerGrain>(shortUrl);

        await using var documentSession = DocumentStore.LightweightSession();

        // Act & Assert
        await grain.SetUrl("https://example.com");

        var document = await documentSession.LoadAsync<OrleansState>($"{ClusterOptions.DefaultServiceId}-url-{grain.GetGrainId()}");
        Assert.NotNull(document);

        document = document with { Data = string.Empty };
        documentSession.Update(document);
        await documentSession.SaveChangesAsync();

        var exception = await Assert.ThrowsAsync<InconsistentStateException>(() => grain.Delete());
        Assert.IsType<ConcurrencyException>(exception.InnerException);
    }
}