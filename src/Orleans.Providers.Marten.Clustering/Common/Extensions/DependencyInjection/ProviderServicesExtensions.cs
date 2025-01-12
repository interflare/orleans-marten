// ReSharper disable CheckNamespace - this is the suggested namespace for dependency injection

using Marten;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Messaging;
using Orleans.Providers.Marten.Clustering;
using Orleans.Providers.Marten.Clustering.Documents;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class ProviderServicesExtensions
{
    /// <summary>
    /// Configures this silo to use Marten for clustering.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="ClusterOptions"/> configured in the DI container are used to set the <see cref="ClusterOptions.ClusterId"/> and <see cref="ClusterOptions.ServiceId"/> for 'tenanting' the cluster storage, which is optional.
    /// </para>
    /// <para>
    /// Expects that Marten is already configured. This method will add its own document types to the Marten configuration - be sure to perform database migrations when this library is added for the first time, or updated.
    /// </para>
    /// </remarks>
    public static ISiloBuilder UseMartenClustering(this ISiloBuilder builder)
        => builder.ConfigureServices(services =>
        {
            services.AddSingleton<IMembershipTable, MartenClusteringTable>();
            services.ConfigureMarten(options => options.ConfigurePersistence());
        });

    /// <summary>
    /// Configures this client to use Marten for clustering.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="ClusterOptions"/> configured in the DI container are used to set the <see cref="ClusterOptions.ClusterId"/> and <see cref="ClusterOptions.ServiceId"/> for 'tenanting' the cluster storage, which is optional.
    /// </para>
    /// <para>
    /// Expects that Marten is already configured. This method will add its own document types to the Marten configuration - be sure to perform database migrations when this library is added for the first time, or updated.
    /// </para>
    /// </remarks>
    public static IClientBuilder UseMartenClustering(this IClientBuilder builder)
        => builder.ConfigureServices(services =>
        {
            services.AddSingleton<IGatewayListProvider, MartenGatewayListProvider>();
            services.ConfigureMarten(options => options.ConfigurePersistence());
        });


    private static StoreOptions ConfigurePersistence(this StoreOptions options)
    {
        options.Schema.For<OrleansClusterVersion>();
        options.Schema.For<OrleansMembership>();

        return options;
    }
}