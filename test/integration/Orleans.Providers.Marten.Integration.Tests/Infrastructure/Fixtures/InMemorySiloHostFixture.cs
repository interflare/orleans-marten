using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Orleans.Providers.Marten.Integration.Tests.Infrastructure.Fixtures;

[ExcludeFromCodeCoverage]
public abstract class InMemorySiloHostFixture : IAsyncLifetime
{
    private IHost _host = null!;

    public IGrainFactory GrainFactory => _host.Services.GetRequiredService<IGrainFactory>();
    public IDocumentStore DocumentStore => _host.Services.GetRequiredService<IDocumentStore>();

    protected virtual void ConfigureOrleansBuilder(ISiloBuilder siloBuilder)
        => siloBuilder.UseLocalhostClustering()
            .AddMemoryGrainStorageAsDefault();

    protected virtual void ConfigureHostBuilder(IHostBuilder hostBuilder)
        => hostBuilder.UseOrleans(ConfigureOrleansBuilder);

    protected virtual void ConfigureServices(IServiceCollection services)
    {
    }

    public virtual Task InitializeAsync()
    {
        var hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureLogging(builder => builder.SetMinimumLevel(LogLevel.Trace))
            .ConfigureServices(ConfigureServices);
        ConfigureHostBuilder(hostBuilder);

        _host = hostBuilder.Build();
        _ = _host.StartAsync();

        return Task.CompletedTask;
    }

    public virtual async Task DisposeAsync() => await _host.StopAsync();
}