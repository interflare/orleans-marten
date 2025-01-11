using Marten;
using Marten.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans.Configuration;
using Orleans.Providers.Marten.Persistence.Common.Extensions;
using Orleans.Providers.Marten.Persistence.Documents;
using Orleans.Runtime;
using Orleans.Storage;
using System.Text.Json;

namespace Orleans.Providers.Marten.Persistence;

/// <summary>
/// Storage provider for writing grain state data to a Marten database store as JSON.
/// </summary>
public sealed class MartenGrainStorage : IGrainStorage
{
    private readonly ILogger<MartenGrainStorage> _logger;
    private readonly IDocumentStore _store;

    private readonly string _serviceId;

    public MartenGrainStorage(ILogger<MartenGrainStorage> logger, IOptions<ClusterOptions> clusterOptions, IDocumentStore store)
    {
        _logger = logger;
        _store = store;

        _serviceId = clusterOptions.Value.ServiceId;
    }

    public async Task ReadStateAsync<T>(string stateName, GrainId grainId, IGrainState<T> grainState)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(stateName);
        ArgumentNullException.ThrowIfNull(grainId);

        await using var querySession = _store.QuerySession();
        var documentId = GetDocumentId(stateName, grainId);
        try
        {
            _logger.LogTraceReading(stateName, grainId, grainState.ETag, documentId);

            var result = await querySession.LoadAsync<OrleansState>(documentId);
            if (result is null)
            {
                _logger.LogTraceDocumentNotFoundReading(stateName, grainId, grainState.ETag, documentId);
                return;
            }

            grainState.ETag = result.Version.ToString();

            if (string.IsNullOrWhiteSpace(result.Data))
            {
                _logger.LogTraceDocumentEmptyReading(stateName, grainId, grainState.ETag, documentId);
                grainState.State = Activator.CreateInstance<T>();
            }
            else grainState.State = JsonSerializer.Deserialize<T>(result.Data) ?? Activator.CreateInstance<T>();

            _logger.LogTraceRead(stateName, grainId, grainState.ETag, documentId);
        }
        catch (Exception e)
        {
            _logger.LogErrorReadingDocument(e, stateName, grainId, grainState.ETag, documentId);
            throw;
        }
    }

    public async Task WriteStateAsync<T>(string stateName, GrainId grainId, IGrainState<T> grainState)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(stateName);
        ArgumentNullException.ThrowIfNull(grainId);

        await using var documentSession = _store.LightweightSession();
        var documentId = GetDocumentId(stateName, grainId);
        Guid? documentETag = null;
        try
        {
            _logger.LogTraceWriting(stateName, grainId, grainState.ETag, documentId);

            var exists = true;
            var result = await documentSession.LoadAsync<OrleansState>(documentId);
            if (result == null)
            {
                _logger.LogTraceDocumentNotFoundWriting(stateName, grainId, grainState.ETag, documentId);
                exists = false;
                result = new OrleansState { Id = documentId, ServiceId = _serviceId, GrainId = grainId.ToString(), StateName = stateName };
            }

            documentETag = result.Version;
            result = result with
            {
                ProviderVersion = Constants.ProviderVersion,
                Data = JsonSerializer.Serialize(grainState.State)
            };

            if (exists) documentSession.UpdateExpectedVersion(result, Guid.Parse(grainState.ETag));
            else documentSession.Insert(result);
            await documentSession.SaveChangesAsync();

            grainState.ETag = result.Version.ToString();
            grainState.RecordExists = true;

            _logger.LogTraceWritten(stateName, grainId, grainState.ETag, documentId);
        }
        catch (ConcurrencyException e)
        {
            throw new InconsistentStateException(documentETag?.ToString(), grainState.ETag, e);
        }
        catch (Exception e)
        {
            _logger.LogErrorWritingDocument(e, stateName, grainId, grainState.ETag, documentId);
            throw;
        }
    }

    public async Task ClearStateAsync<T>(string stateName, GrainId grainId, IGrainState<T> grainState)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(stateName);
        ArgumentNullException.ThrowIfNull(grainId);

        await using var documentSession = _store.LightweightSession();
        var documentId = GetDocumentId(stateName, grainId);
        Guid? documentETag = null;
        try
        {
            _logger.LogTraceClearing(stateName, grainId, grainState.ETag, documentId);

            var result = await documentSession.LoadAsync<OrleansState>(documentId);
            if (result is null)
            {
                _logger.LogTraceDocumentNotFoundClearing(stateName, grainId, grainState.ETag, documentId);
                grainState.ETag = null;
                grainState.RecordExists = false;
                return;
            }

            documentETag = result.Version;
            if (documentETag.ToString() != grainState.ETag) throw new ConcurrencyException(typeof(OrleansState), documentId);

            documentSession.Delete(result);
            await documentSession.SaveChangesAsync();

            grainState.ETag = result.Version.ToString();
            grainState.RecordExists = false;

            _logger.LogTraceCleared(stateName, grainId, grainState.ETag, documentId);
        }
        catch (ConcurrencyException e)
        {
            throw new InconsistentStateException(documentETag?.ToString(), grainState.ETag, e);
        }
        catch (Exception e)
        {
            _logger.LogErrorClearingDocument(e, stateName, grainId, grainState.ETag, documentId);
            throw;
        }
    }

    private string GetDocumentId(string stateName, GrainId grainId) => $"{_serviceId}-{stateName}-{grainId}";
}