using Marten;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans.Configuration;
using Orleans.Messaging;
using Orleans.Providers.Marten.Clustering.Common.Extensions;
using Orleans.Providers.Marten.Clustering.Documents;
using Orleans.Runtime;

namespace Orleans.Providers.Marten.Clustering;

/// <summary>
/// Gateway provider for listing silos managed by the Marten database store.
/// </summary>
/// <seealso cref="MartenClusteringTable"/>
public class MartenGatewayListProvider : IGatewayListProvider
{
    private readonly ILogger<MartenGatewayListProvider> _logger;
    private readonly IDocumentStore _store;

    private readonly string _serviceId;
    private readonly string _clusterId;

    public MartenGatewayListProvider(ILogger<MartenGatewayListProvider> logger, IOptions<ClusterOptions> clusterOptions, IDocumentStore store)
    {
        _logger = logger;
        _store = store;

        _serviceId = clusterOptions.Value.ServiceId;
        _clusterId = clusterOptions.Value.ClusterId;
    }

    // ReSharper disable once UnassignedGetOnlyAutoProperty
    public TimeSpan MaxStaleness => TimeSpan.FromMinutes(1);
    public bool IsUpdatable => true;

    public Task InitializeGatewayListProvider() => Task.CompletedTask;

    public async Task<IList<Uri>> GetGateways()
    {
        await using var querySession = _store.QuerySession();
        try
        {
            _logger.LogTraceListingGateways(_clusterId);

            var members = await querySession.Query<OrleansMembership>()
                .Where(m => m.ServiceId == _serviceId && m.ClusterId == _clusterId)
                .Where(m => m.Entry.Status == SiloStatus.Active)
                .Where(m => m.Entry.ProxyPort > 0)
                .Select(m => m.Entry)
                .ToListAsync();

            var results = members.Select(m => m.SiloAddress.ToGatewayUri()).ToList();

            _logger.LogTraceListedGateways(_clusterId);
            return results;
        }
        catch (Exception e)
        {
            _logger.LogErrorListingGateways(e, _clusterId);
            throw;
        }
    }
}