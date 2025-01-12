using Marten;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans.Configuration;
using Orleans.Providers.Marten.Reminders.Common.Extensions;
using Orleans.Providers.Marten.Reminders.Common.Mappers;
using Orleans.Providers.Marten.Reminders.Documents;

namespace Orleans.Providers.Marten.Reminders;

/// <summary>
/// Reminder table provider for persisting reminders to a Marten database store.
/// </summary>
public class MartenReminderTable : IReminderTable
{
    private readonly ILogger<MartenReminderTable> _logger;
    private readonly IDocumentStore _store;

    private readonly string _serviceId;
    private readonly string _clusterId;

    public MartenReminderTable(ILogger<MartenReminderTable> logger, IOptions<ClusterOptions> clusterOptions, IDocumentStore store)
    {
        _logger = logger;
        _store = store;

        _serviceId = clusterOptions.Value.ServiceId;
        _clusterId = clusterOptions.Value.ClusterId;
    }

    public async Task<ReminderTableData> ReadRows(GrainId grainId)
    {
        ArgumentNullException.ThrowIfNull(grainId);

        await using var querySession = _store.QuerySession();
        try
        {
            _logger.LogTraceReadingGrainRows(grainId);

            var results = await querySession.Query<OrleansReminder>()
                .Where(r => r.ServiceId == _serviceId)
                .Where(r => r.GrainId == grainId.ToString())
                .ToListAsync();

            _logger.LogTraceReadGrainRows(grainId);
            return new ReminderTableData(results.Select(ReminderMappers.MapToNative));
        }
        catch (Exception e)
        {
            _logger.LogErrorReadingGrainRows(e, grainId);
            throw;
        }
    }

    public async Task<ReminderTableData> ReadRows(uint begin, uint end)
    {
        await using var querySession = _store.QuerySession();
        try
        {
            _logger.LogTraceReadingRangeRows(begin, end);
            var query = querySession.Query<OrleansReminder>()
                .Where(r => r.ServiceId == _serviceId);

            query = begin < end
                ? query.Where(r => r.GrainHash > begin && r.GrainHash <= end)
                : query.Where(r => r.GrainHash > begin || r.GrainHash <= end);

            var results = await query.ToListAsync();

            _logger.LogTraceReadRangeRows(begin, end);
            return new ReminderTableData(results.Select(ReminderMappers.MapToNative));
        }
        catch (Exception e)
        {
            _logger.LogErrorReadingRangeRows(e, begin, end);
            throw;
        }
    }

    public async Task<ReminderEntry> ReadRow(GrainId grainId, string reminderName)
    {
        ArgumentNullException.ThrowIfNull(grainId);
        ArgumentException.ThrowIfNullOrWhiteSpace(reminderName);

        await using var querySession = _store.QuerySession();
        var documentId = GetDocumentId(grainId, reminderName);
        try
        {
            _logger.LogTraceReadingRow(grainId, reminderName);

            var result = await querySession.LoadAsync<OrleansReminder>(documentId);

            _logger.LogTraceReadRow(grainId, reminderName);
            return result?.MapToNative()!;
        }
        catch (Exception e)
        {
            _logger.LogErrorReadingRow(e, grainId, reminderName);
            throw;
        }
    }

    public async Task<string> UpsertRow(ReminderEntry entry)
    {
        ArgumentNullException.ThrowIfNull(entry);
        ArgumentException.ThrowIfNullOrWhiteSpace(entry.ReminderName);

        if (entry.StartAt.Kind is DateTimeKind.Unspecified) entry.StartAt = DateTime.SpecifyKind(entry.StartAt, DateTimeKind.Utc);

        await using var documentSession = _store.LightweightSession();
        var documentId = GetDocumentId(entry.GrainId, entry.ReminderName);
        try
        {
            _logger.LogTraceUpsertingRow(entry.GrainId, entry.ReminderName, entry.ETag);

            var exists = true;
            var result = await documentSession.LoadAsync<OrleansReminder>(documentId);
            if (result == null)
            {
                _logger.LogTraceDocumentNotFoundUpsertingRow(entry.GrainId, entry.ReminderName, entry.ETag);
                exists = false;
                result = new OrleansReminder { Id = documentId, ServiceId = _serviceId };
            }

            result = result with
            {
                ClusterId = _clusterId,
                ReminderName = entry.ReminderName,
                GrainId = entry.GrainId.ToString(),
                GrainHash = entry.GrainId.GetUniformHashCode(),
                StartAt = entry.StartAt,
                Period = entry.Period,
                ProviderVersion = Constants.ProviderVersion
            };

            if (exists) documentSession.UpdateExpectedVersion(result, Guid.Parse(entry.ETag));
            else documentSession.Insert(result);
            await documentSession.SaveChangesAsync();

            _logger.LogTraceUpsertedRow(entry.GrainId, entry.ReminderName, result.Version.ToString());
            return result.Version.ToString();
        }
        catch (Exception e)
        {
            _logger.LogErrorUpsertingRow(e, entry.GrainId, entry.ReminderName, entry.ETag);
            throw;
        }
    }

    public async Task<bool> RemoveRow(GrainId grainId, string reminderName, string eTag)
    {
        ArgumentNullException.ThrowIfNull(grainId);
        ArgumentException.ThrowIfNullOrWhiteSpace(reminderName);
        ArgumentException.ThrowIfNullOrWhiteSpace(eTag);

        await using var documentSession = _store.LightweightSession();
        var documentId = GetDocumentId(grainId, reminderName);
        try
        {
            _logger.LogTraceRemovingRow(grainId, reminderName, eTag);

            var result = await documentSession.LoadAsync<OrleansReminder>(documentId);
            if (result == null)
            {
                _logger.LogTraceDocumentNotFoundRemovingRow(grainId, reminderName, eTag);
                return false;
            }

            if (result.Version.ToString() != eTag)
            {
                _logger.LogTraceETagMismatchRemovingRow(grainId, reminderName, eTag, result.Version.ToString());
                return false;
            }

            documentSession.Delete(documentId);
            await documentSession.SaveChangesAsync();

            _logger.LogTraceRemovedRow(grainId, reminderName, eTag);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogErrorRemovingRow(e, grainId, reminderName, eTag);
            throw;
        }
    }

    /// <inheritdoc />
    /// <remarks>
    /// Only clears the reminder records for the contextual service identifier.
    /// </remarks>
    public async Task TestOnlyClearTable()
    {
        _logger.LogWarning("Clearing reminders table: ServiceId={ServiceId}", _serviceId);
        await using var documentSession = _store.LightweightSession();
        documentSession.DeleteWhere<OrleansReminder>(r => r.ServiceId == _serviceId);
        await documentSession.SaveChangesAsync();
    }

    private string GetDocumentId(GrainId grainId, string reminderName) => $"{_serviceId}-{grainId}-{reminderName}";
}