// ReSharper disable CheckNamespace - this is the suggested namespace for dependency injection

using Marten;
using Microsoft.Extensions.Options;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Providers;
using Orleans.Providers.Marten.Persistence;
using Orleans.Providers.Marten.Persistence.Documents;
using Orleans.Runtime.Hosting;
using Orleans.Storage;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class ProviderServicesExtensions
{
    /// <summary>
    /// Configures the silo to use Marten grain state storage as a named provider.
    /// </summary>
    /// <param name="services">The service collection being configured.</param>
    /// <param name="name">The name of the state storage provider.</param>
    /// <remarks>
    /// <para>
    /// The <see cref="ClusterOptions"/> configured in the DI container are used to set the <see cref="ClusterOptions.ServiceId"/> for 'tenanting' the cluster storage, which is optional.
    /// </para>
    /// <para>
    /// Expects that Marten is already configured. This method will add its own document types to the Marten configuration - be sure to perform database migrations when this library is added for the first time, or updated.
    /// </para>
    /// </remarks>
    public static IServiceCollection AddMartenGrainStorage(this IServiceCollection services, string name)
    {
        services.ConfigureMarten(options => options.ConfigurePersistence());
        services.AddOptions<MartenGrainStorageOptions>(name);
        services.AddTransient<IPostConfigureOptions<MartenGrainStorageOptions>, DefaultStorageProviderSerializerOptionsConfigurator<MartenGrainStorageOptions>>();
        return services.AddGrainStorage(name, MartenGrainStorageFactory.Create);
    }

    /// <summary>
    /// Configures the silo to use Marten grain state storage as a named provider.
    /// </summary>
    /// <param name="builder">The silo being configured.</param>
    /// <param name="name">The name of the state storage provider.</param>
    /// <remarks>
    /// <para>
    /// The <see cref="ClusterOptions"/> configured in the DI container are used to set the <see cref="ClusterOptions.ServiceId"/> for 'tenanting' the cluster storage, which is optional.
    /// </para>
    /// <para>
    /// Expects that Marten is already configured. This method will add its own document types to the Marten configuration - be sure to perform database migrations when this library is added for the first time, or updated.
    /// </para>
    /// </remarks>
    public static ISiloBuilder AddMartenGrainStorage(this ISiloBuilder builder, string name)
        => builder.ConfigureServices(services => services.AddMartenGrainStorage(name));

    /// <summary>
    /// Configures the silo to use Marten grain state storage as the default provider.
    /// </summary>
    /// <param name="services">The service collection being configured.</param>
    /// <remarks>
    /// <para>
    /// Note that only one grain state storage provider can be configured as the default across the silo - the first registered wins.
    /// </para>
    /// <para>
    /// The <see cref="ClusterOptions"/> configured in the DI container are used to set the <see cref="ClusterOptions.ServiceId"/> for 'tenanting' the cluster storage, which is optional.
    /// </para>
    /// <para>
    /// Expects that Marten is already configured. This method will add its own document types to the Marten configuration - be sure to perform database migrations when this library is added for the first time, or updated.
    /// </para>
    /// </remarks>
    public static IServiceCollection AddMartenGrainStorageAsDefault(this IServiceCollection services)
        => services.AddMartenGrainStorage(ProviderConstants.DEFAULT_STORAGE_PROVIDER_NAME);

    /// <summary>
    /// Configures the silo to use Marten grain state storage as the default provider.
    /// </summary>
    /// <param name="builder">The silo being configured.</param>
    /// <remarks>
    /// <para>
    /// Note that only one grain state storage provider can be configured as the default across the silo - the first registered wins.
    /// </para>
    /// <para>
    /// The <see cref="ClusterOptions"/> configured in the DI container are used to set the <see cref="ClusterOptions.ServiceId"/> for 'tenanting' the cluster storage, which is optional.
    /// </para>
    /// <para>
    /// Expects that Marten is already configured. This method will add its own document types to the Marten configuration - be sure to perform database migrations when this library is added for the first time, or updated.
    /// </para>
    /// </remarks>
    public static ISiloBuilder AddMartenGrainStorageAsDefault(this ISiloBuilder builder)
        => builder.AddMartenGrainStorage(ProviderConstants.DEFAULT_STORAGE_PROVIDER_NAME);


    private static StoreOptions ConfigurePersistence(this StoreOptions options)
    {
        options.Schema.For<OrleansState>();

        return options;
    }
}