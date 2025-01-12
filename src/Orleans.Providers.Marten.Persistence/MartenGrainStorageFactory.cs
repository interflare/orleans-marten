using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Orleans.Configuration.Overrides;
using System.Diagnostics.CodeAnalysis;

namespace Orleans.Providers.Marten.Persistence;

[ExcludeFromCodeCoverage]
public static class MartenGrainStorageFactory
{
    public static MartenGrainStorage Create(IServiceProvider services, string name)
    {
        var optionsMonitor = services.GetRequiredService<IOptionsMonitor<MartenGrainStorageOptions>>();
        return ActivatorUtilities.CreateInstance<MartenGrainStorage>(services, optionsMonitor.Get(name), services.GetProviderClusterOptions(name));
    }
}