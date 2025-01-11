using Marten.Exceptions;
using Orleans.Configuration;
using Orleans.Providers.Marten.Integration.Tests.Infrastructure.Fixtures;
using Orleans.Providers.Marten.Integration.Tests.TestGrains;
using Orleans.Providers.Marten.Persistence.Documents;
using Orleans.Storage;
using System.Text.Json;

namespace Orleans.Providers.Marten.Integration.Tests;

public class PersistenceTests : IClassFixture<MartenStatePersistedSiloHostFixture>
{
    private readonly MartenStatePersistedSiloHostFixture _siloHostFixture;

    public PersistenceTests(MartenStatePersistedSiloHostFixture siloHostFixture)
    {
        _siloHostFixture = siloHostFixture;
    }

    [Fact]
    public async Task StatePersistenceReadWrite()
    {
        // Arrange
        var shortUrl = Guid.NewGuid().GetHashCode().ToString("x");
        var grain = _siloHostFixture.GrainFactory.GetGrain<IUrlShortenerGrain>(shortUrl);

        await using var documentSession = _siloHostFixture.DocumentStore.LightweightSession();

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
        var grain = _siloHostFixture.GrainFactory.GetGrain<IUrlShortenerGrain>(shortUrl);

        await using var documentSession = _siloHostFixture.DocumentStore.LightweightSession();

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
        var grain = _siloHostFixture.GrainFactory.GetGrain<IUrlShortenerGrain>(shortUrl);

        await using var documentSession = _siloHostFixture.DocumentStore.LightweightSession();

        // Act & Assert
        await grain.SetUrl("https://example.com");

        var document = await documentSession.LoadAsync<OrleansState>($"{ClusterOptions.DefaultServiceId}-url-{grain.GetGrainId()}");
        Assert.NotNull(document);

        var documentETag = document.Version.ToString();
        document = document with { Data = string.Empty };
        documentSession.Update(document);
        await documentSession.SaveChangesAsync();

        var exception = await Assert.ThrowsAsync<InconsistentStateException>(() => grain.SetUrl("https://example.net"));
        Assert.IsType<ConcurrencyException>(exception.InnerException);
        Assert.Equal(documentETag, exception.CurrentEtag);
        Assert.Equal(document.Version.ToString(), exception.StoredEtag);
    }

    [Fact]
    public async Task StateConcurrencyDeletion()
    {
        // Arrange
        var shortUrl = Guid.NewGuid().GetHashCode().ToString("x");
        var grain = _siloHostFixture.GrainFactory.GetGrain<IUrlShortenerGrain>(shortUrl);

        await using var documentSession = _siloHostFixture.DocumentStore.LightweightSession();

        // Act & Assert
        await grain.SetUrl("https://example.com");

        var document = await documentSession.LoadAsync<OrleansState>($"{ClusterOptions.DefaultServiceId}-url-{grain.GetGrainId()}");
        Assert.NotNull(document);

        var documentETag = document.Version.ToString();
        document = document with { Data = string.Empty };
        documentSession.Update(document);
        await documentSession.SaveChangesAsync();

        var exception = await Assert.ThrowsAsync<InconsistentStateException>(() => grain.Delete());
        Assert.IsType<ConcurrencyException>(exception.InnerException);
        Assert.Equal(documentETag, exception.CurrentEtag);
        Assert.Equal(document.Version.ToString(), exception.StoredEtag);
    }
}