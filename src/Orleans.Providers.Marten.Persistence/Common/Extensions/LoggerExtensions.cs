using Microsoft.Extensions.Logging;
using Orleans.Runtime;

namespace Orleans.Providers.Marten.Persistence.Common.Extensions;

internal static partial class LoggerExtensions
{
    #region Reading

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Reading: StateName={StateName} GrainId={GrainId} ETag={ETag} DocumentId={DocumentId}")]
    internal static partial void LogTraceReading(this ILogger logger, string stateName, GrainId grainId, string? eTag, string documentId);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Read: StateName={StateName} GrainId={GrainId} ETag={ETag} DocumentId={DocumentId}")]
    internal static partial void LogTraceRead(this ILogger logger, string stateName, GrainId grainId, string? eTag, string documentId);


    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Error reading: StateName={StateName} GrainId={GrainId} ETag={ETag} DocumentId={DocumentId}")]
    internal static partial void LogErrorReadingDocument(this ILogger logger, Exception ex, string stateName, GrainId grainId, string? eTag, string documentId);


    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "DocumentEmpty reading: StateName={StateName} GrainId={GrainId} ETag={ETag} DocumentId={DocumentId}")]
    internal static partial void LogTraceDocumentEmptyReading(this ILogger logger, string stateName, GrainId grainId, string? eTag, string documentId);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "DocumentNotFound reading: StateName={StateName} GrainId={GrainId} ETag={ETag} DocumentId={DocumentId}")]
    internal static partial void LogTraceDocumentNotFoundReading(this ILogger logger, string stateName, GrainId grainId, string? eTag, string documentId);

    #endregion


    #region Writing

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Writing: StateName={StateName} GrainId={GrainId} ETag={ETag} DocumentId={DocumentId}")]
    internal static partial void LogTraceWriting(this ILogger logger, string stateName, GrainId grainId, string? eTag, string documentId);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Written: StateName={StateName} GrainId={GrainId} ETag={ETag} DocumentId={DocumentId}")]
    internal static partial void LogTraceWritten(this ILogger logger, string stateName, GrainId grainId, string? eTag, string documentId);


    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Error writing: StateName={StateName} GrainId={GrainId} ETag={ETag} DocumentId={DocumentId}")]
    internal static partial void LogErrorWritingDocument(this ILogger logger, Exception ex, string stateName, GrainId grainId, string? eTag, string documentId);


    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "DocumentNotFound writing: StateName={StateName} GrainId={GrainId} ETag={ETag} DocumentId={DocumentId}")]
    internal static partial void LogTraceDocumentNotFoundWriting(this ILogger logger, string stateName, GrainId grainId, string? eTag, string documentId);

    #endregion


    #region Clearing

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Clearing: StateName={StateName} GrainId={GrainId} ETag={ETag} DocumentId={DocumentId}")]
    internal static partial void LogTraceClearing(this ILogger logger, string stateName, GrainId grainId, string? eTag, string documentId);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Cleared: StateName={StateName} GrainId={GrainId} ETag={ETag} DocumentId={DocumentId}")]
    internal static partial void LogTraceCleared(this ILogger logger, string stateName, GrainId grainId, string? eTag, string documentId);


    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Error clearing: StateName={StateName} GrainId={GrainId} ETag={ETag} DocumentId={DocumentId}")]
    internal static partial void LogErrorClearingDocument(this ILogger logger, Exception ex, string stateName, GrainId grainId, string? eTag, string documentId);


    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "DocumentNotFound clearing: StateName={StateName} GrainId={GrainId} ETag={ETag} DocumentId={DocumentId}")]
    internal static partial void LogTraceDocumentNotFoundClearing(this ILogger logger, string stateName, GrainId grainId, string? eTag, string documentId);

    #endregion
}