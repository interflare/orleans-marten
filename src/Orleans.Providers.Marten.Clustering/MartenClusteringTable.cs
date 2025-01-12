using Marten;
using Marten.Exceptions;
using Marten.Patching;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans.Configuration;
using Orleans.Providers.Marten.Clustering.Common.Extensions;
using Orleans.Providers.Marten.Clustering.Common.Mappers;
using Orleans.Providers.Marten.Clustering.Documents;
using Orleans.Runtime;

namespace Orleans.Providers.Marten.Clustering;

/// <summary>
/// Membership table provider for managing silo clustering to a Marten database store.
/// </summary>
/// <seealso cref="MartenGatewayListProvider"/>
public class MartenClusteringTable : IMembershipTable
{
    private readonly ILogger<MartenClusteringTable> _logger;
    private readonly IDocumentStore _store;

    private readonly string _serviceId;
    private readonly string _clusterId;

    public MartenClusteringTable(ILogger<MartenClusteringTable> logger, IOptions<ClusterOptions> clusterOptions, IDocumentStore store)
    {
        _logger = logger;
        _store = store;

        _serviceId = clusterOptions.Value.ServiceId;
        _clusterId = clusterOptions.Value.ClusterId;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Not used by this provider.
    /// </remarks>
    public Task InitializeMembershipTable(bool tryInitTableVersion) => Task.CompletedTask;

    public async Task DeleteMembershipTableEntries(string clusterId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(clusterId);

        await using var documentSession = _store.LightweightSession();
        try
        {
            _logger.LogTraceDeleting(clusterId);

            documentSession.DeleteWhere<OrleansMembership>(m => m.ServiceId == _serviceId && m.ClusterId == clusterId);
            await documentSession.SaveChangesAsync();

            _logger.LogTraceDeleted(clusterId);
        }
        catch (Exception e)
        {
            _logger.LogErrorDeleting(e, clusterId);
            throw;
        }
    }

    public async Task CleanupDefunctSiloEntries(DateTimeOffset beforeDate)
    {
        await using var documentSession = _store.LightweightSession();
        try
        {
            _logger.LogTraceCleaning(_clusterId, beforeDate);

            documentSession.DeleteWhere<OrleansMembership>(m => m.ServiceId == _serviceId && m.ClusterId == _clusterId && m.Entry.IAmAliveTime <= beforeDate);
            await documentSession.SaveChangesAsync();

            _logger.LogTraceCleaned(_clusterId, beforeDate);
        }
        catch (Exception e)
        {
            _logger.LogErrorCleaning(e, _clusterId, beforeDate);
            throw;
        }
    }

    public async Task<MembershipTableData> ReadRow(SiloAddress key)
    {
        await using var querySession = _store.QuerySession();
        var documentId = GetDocumentId(key);
        try
        {
            _logger.LogTraceReadingRow(_clusterId, key.ToParsableString());

            var membership = await querySession.LoadAsync<OrleansMembership>(documentId);
            if (membership == null) _logger.LogTraceDocumentNotFoundReadingRow(_clusterId, key.ToParsableString());
            var result = await GetMembershipTableData(querySession, [membership]);

            _logger.LogTraceReadRow(_clusterId, key.ToParsableString());
            return result;
        }
        catch (Exception e)
        {
            _logger.LogErrorReadingRow(e, _clusterId, key.ToParsableString());
            throw;
        }
    }

    public async Task<MembershipTableData> ReadAll()
    {
        await using var querySession = _store.QuerySession();
        try
        {
            _logger.LogTraceReadingAllRows(_clusterId);

            var memberships = await querySession.Query<OrleansMembership>()
                .Where(m => m.ServiceId == _serviceId && m.ClusterId == _clusterId)
                .ToListAsync();
            var results = await GetMembershipTableData(querySession, memberships);

            _logger.LogTraceReadAllRows(_clusterId);
            return results;
        }
        catch (Exception e)
        {
            _logger.LogErrorReadingAllRows(e, _clusterId);
            throw;
        }
    }

    public async Task<bool> InsertRow(MembershipEntry entry, TableVersion tableVersion)
    {
        await using var documentSession = _store.LightweightSession();
        var tableVersionId = GetTableVersionId();
        var documentId = GetDocumentId(entry.SiloAddress);
        try
        {
            _logger.LogTraceInsertingRow(_clusterId, entry.SiloAddress.ToParsableString(), tableVersion.VersionEtag);

            var storedTableVersion = await documentSession.LoadAsync<OrleansClusterVersion>(tableVersionId);
            if (storedTableVersion == null)
            {
                _logger.LogTraceDocumentNotFoundInsertingRow(_clusterId, entry.SiloAddress.ToParsableString(), tableVersion.VersionEtag);
                documentSession.Insert(new OrleansClusterVersion { Id = tableVersionId, ProviderVersion = Constants.ProviderVersion, Revision = tableVersion.Version });
            }
            else
            {
                storedTableVersion = storedTableVersion with { ProviderVersion = Constants.ProviderVersion, Revision = tableVersion.Version };
                documentSession.UpdateExpectedVersion(storedTableVersion, Guid.Parse(tableVersion.VersionEtag));
            }

            documentSession.Insert(new OrleansMembership
            {
                Id = documentId,
                ProviderVersion = Constants.ProviderVersion,
                ServiceId = _serviceId,
                ClusterId = _clusterId,
                Entry = entry.MapToModel()
            });

            await documentSession.SaveChangesAsync();

            _logger.LogTraceInsertedRow(_clusterId, entry.SiloAddress.ToParsableString(), tableVersion.VersionEtag);
            return true;
        }
        catch (ConcurrencyException e)
        {
            _logger.LogErrorETagMismatchInsertingRow(e, _clusterId, entry.SiloAddress.ToParsableString(), tableVersion.VersionEtag);
            return false;
        }
        catch (DocumentAlreadyExistsException e)
        {
            _logger.LogErrorDocumentAlreadyExistsInsertingRow(e, _clusterId, entry.SiloAddress.ToParsableString(), tableVersion.VersionEtag);
            return false;
        }
        catch (Exception e)
        {
            _logger.LogErrorInsertingRow(e, _clusterId, entry.SiloAddress.ToParsableString(), tableVersion.VersionEtag);
            throw;
        }
    }

    public async Task<bool> UpdateRow(MembershipEntry entry, string etag, TableVersion tableVersion)
    {
        await using var documentSession = _store.LightweightSession();
        var tableVersionId = GetTableVersionId();
        var documentId = GetDocumentId(entry.SiloAddress);
        try
        {
            _logger.LogTraceUpdatingRow(_clusterId, entry.SiloAddress.ToParsableString(), etag, tableVersion.VersionEtag);

            var storedTableVersion = await documentSession.LoadAsync<OrleansClusterVersion>(tableVersionId);
            if (storedTableVersion == null)
            {
                _logger.LogErrorTableVersionDocumentNotFoundUpdatingRow(_clusterId, entry.SiloAddress.ToParsableString(), etag, tableVersion.VersionEtag);
                return false;
            }

            storedTableVersion = storedTableVersion with { ProviderVersion = Constants.ProviderVersion, Revision = tableVersion.Version };
            documentSession.UpdateExpectedVersion(storedTableVersion, Guid.Parse(tableVersion.VersionEtag));

            var membership = await documentSession.LoadAsync<OrleansMembership>(documentId);
            if (membership == null)
            {
                _logger.LogErrorMemberDocumentNotFoundUpdatingRow(_clusterId, entry.SiloAddress.ToParsableString(), etag, tableVersion.VersionEtag);
                return false;
            }

            membership = membership with { ProviderVersion = Constants.ProviderVersion, Entry = entry.MapToModel() };
            documentSession.UpdateExpectedVersion(membership, Guid.Parse(etag));

            await documentSession.SaveChangesAsync();

            _logger.LogTraceUpdatedRow(_clusterId, entry.SiloAddress.ToParsableString(), etag, tableVersion.VersionEtag);
            return true;
        }
        catch (ConcurrencyException e) when (e.DocType == nameof(OrleansClusterVersion))
        {
            if (e.DocType == nameof(OrleansClusterVersion)) _logger.LogErrorTableVersionETagMismatchUpdatingRow(e, _clusterId, entry.SiloAddress.ToParsableString(), etag, tableVersion.VersionEtag);
            if (e.DocType == nameof(OrleansMembership)) _logger.LogErrorMemberETagMismatchUpdatingRow(e, _clusterId, entry.SiloAddress.ToParsableString(), etag, tableVersion.VersionEtag);
            return false;
        }
        catch (Exception e)
        {
            _logger.LogErrorUpdatingRow(e, _clusterId, entry.SiloAddress.ToParsableString(), etag, tableVersion.VersionEtag);
            throw;
        }
    }

    public async Task UpdateIAmAlive(MembershipEntry entry)
    {
        await using var documentSession = _store.LightweightSession();
        var documentId = GetDocumentId(entry.SiloAddress);
        try
        {
            _logger.LogTraceUpdatingIAmAlive(_clusterId, entry.SiloAddress.ToParsableString(), entry.IAmAliveTime);

            documentSession.Patch<OrleansMembership>(documentId).Set(m => m.Entry.IAmAliveTime, entry.IAmAliveTime);
            await documentSession.SaveChangesAsync();

            _logger.LogTraceUpdatedIAmAlive(_clusterId, entry.SiloAddress.ToParsableString(), entry.IAmAliveTime);
        }
        catch (Exception e)
        {
            _logger.LogErrorUpdatingIAmAlive(e, _clusterId, entry.SiloAddress.ToParsableString(), entry.IAmAliveTime);
            throw;
        }
    }

    private string GetTableVersionId() => $"{_serviceId}/{_clusterId}";
    private string GetDocumentId(SiloAddress siloAddress) => $"{_serviceId}/{_clusterId}-{siloAddress.ToParsableString()}";

    private async Task<MembershipTableData> GetMembershipTableData(IQuerySession session, IEnumerable<OrleansMembership?> memberships)
    {
        var tableVersionResult = await session.LoadAsync<OrleansClusterVersion>(GetTableVersionId());
        if (tableVersionResult == null) _logger.LogTraceDocumentNotFoundReadingClusterVersion(_clusterId);
        var tableVersion = new TableVersion(tableVersionResult?.Revision ?? 0, (tableVersionResult?.Version ?? Guid.Empty).ToString());

        return new MembershipTableData(memberships
                .Where(m => m != null)
                .Select(m => new Tuple<MembershipEntry, string>(m!.Entry.MapToNative(), m.Version.ToString()))
                .ToList(),
            tableVersion);
    }
}