namespace Orleans.Providers.Marten.Integration.Tests.TestGrains;

public interface IUrlShortenerGrain : IGrainWithStringKey
{
    Task Delete();
    Task SetUrl(string fullUrl);
    Task<string> GetUrl();
}

public sealed class UrlShortenerGrain : Grain, IUrlShortenerGrain
{
    private readonly IPersistentState<UrlDetails> _state;

    public UrlShortenerGrain([PersistentState(stateName: "url")] IPersistentState<UrlDetails> state)
    {
        _state = state;
    }

    public Task Delete() => _state.ClearStateAsync();

    public async Task SetUrl(string fullUrl)
    {
        _state.State = new UrlDetails
        {
            ShortenedRouteSegment = this.GetPrimaryKeyString(),
            FullUrl = fullUrl
        };

        await _state.WriteStateAsync();
    }

    public Task<string> GetUrl() => Task.FromResult(_state.State.FullUrl);
}

[GenerateSerializer, Alias(nameof(UrlDetails))]
public sealed record UrlDetails
{
    [Id(0)] public string FullUrl { get; set; } = "";

    [Id(1)] public string ShortenedRouteSegment { get; set; } = "";
}