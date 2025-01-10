using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Orleans.Providers.Marten.Persistence;

[ExcludeFromCodeCoverage]
public static class MartenGrainStorageFactory
{
    public static MartenGrainStorage Create(IServiceProvider services, string name)
        => ActivatorUtilities.CreateInstance<MartenGrainStorage>(services);
}