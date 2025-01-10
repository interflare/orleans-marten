// ReSharper disable CheckNamespace - this is the suggested namespace for dependency injection

using Marten;
using Orleans.Hosting;
using Orleans.Providers;
using Orleans.Providers.Marten.Persistence;
using Orleans.Providers.Marten.Persistence.Documents;
using Orleans.Runtime.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class ProviderServicesExtensions
{
    public static IServiceCollection AddMartenGrainStorage(this IServiceCollection services, string name)
    {
        services.ConfigureMarten(options => options.ConfigurePersistence());
        return services.AddGrainStorage(name, MartenGrainStorageFactory.Create);
    }

    public static IServiceCollection AddMartenGrainStorageAsDefault(this IServiceCollection services)
        => services.AddMartenGrainStorage(ProviderConstants.DEFAULT_STORAGE_PROVIDER_NAME);

    public static ISiloBuilder AddMartenGrainStorageAsDefault(this ISiloBuilder builder)
        => builder.AddMartenGrainStorage(ProviderConstants.DEFAULT_STORAGE_PROVIDER_NAME);

    public static ISiloBuilder AddMartenGrainStorage(this ISiloBuilder builder, string name)
        => builder.ConfigureServices(services => services.AddMartenGrainStorage(name));


    private static StoreOptions ConfigurePersistence(this StoreOptions options)
    {
        options.Schema.For<OrleansState>();

        return options;
    }
}