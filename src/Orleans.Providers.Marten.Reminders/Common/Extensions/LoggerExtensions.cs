using Microsoft.Extensions.Logging;

namespace Orleans.Providers.Marten.Reminders.Common.Extensions;

internal static partial class LoggerExtensions
{
    #region ReadRowsGrain

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Reading grain rows: GrainId={GrainId}")]
    internal static partial void LogTraceReadingGrainRows(this ILogger logger, GrainId grainId);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Read grain rows: GrainId={GrainId}")]
    internal static partial void LogTraceReadGrainRows(this ILogger logger, GrainId grainId);


    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Error reading grain rows: GrainId={GrainId}")]
    internal static partial void LogErrorReadingGrainRows(this ILogger logger, Exception ex, GrainId grainId);

    #endregion

    #region ReadRowsRange

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Reading range rows: From={From} To={To}")]
    internal static partial void LogTraceReadingRangeRows(this ILogger logger, uint from, uint to);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Read range rows: From={From} To={To}")]
    internal static partial void LogTraceReadRangeRows(this ILogger logger, uint from, uint to);


    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Error reading range rows: From={From} To={To}")]
    internal static partial void LogErrorReadingRangeRows(this ILogger logger, Exception ex, uint from, uint to);

    #endregion

    #region ReadRow

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Reading row: GrainId={GrainId} ReminderName={ReminderName}")]
    internal static partial void LogTraceReadingRow(this ILogger logger, GrainId grainId, string reminderName);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Read row: GrainId={GrainId} ReminderName={ReminderName}")]
    internal static partial void LogTraceReadRow(this ILogger logger, GrainId grainId, string reminderName);


    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Error reading row: GrainId={GrainId} ReminderName={ReminderName}")]
    internal static partial void LogErrorReadingRow(this ILogger logger, Exception ex, GrainId grainId, string reminderName);

    #endregion

    #region UpsertRow

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Upserting row: GrainId={GrainId} ReminderName={ReminderName} ETag={ETag}")]
    internal static partial void LogTraceUpsertingRow(this ILogger logger, GrainId grainId, string reminderName, string eTag);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Upserted row: GrainId={GrainId} ReminderName={ReminderName} ETag={ETag}")]
    internal static partial void LogTraceUpsertedRow(this ILogger logger, GrainId grainId, string reminderName, string eTag);


    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "DocumentNotFound upserting row: GrainId={GrainId} ReminderName={ReminderName} ETag={ETag}")]
    internal static partial void LogTraceDocumentNotFoundUpsertingRow(this ILogger logger, GrainId grainId, string reminderName, string eTag);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Error upserting row: GrainId={GrainId} ReminderName={ReminderName} ETag={ETag}")]
    internal static partial void LogErrorUpsertingRow(this ILogger logger, Exception ex, GrainId grainId, string reminderName, string eTag);

    #endregion

    #region RemoveRow

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Removing row: GrainId={GrainId} ReminderName={ReminderName} ETag={ETag}")]
    internal static partial void LogTraceRemovingRow(this ILogger logger, GrainId grainId, string reminderName, string eTag);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Removed row: GrainId={GrainId} ReminderName={ReminderName} ETag={ETag}")]
    internal static partial void LogTraceRemovedRow(this ILogger logger, GrainId grainId, string reminderName, string eTag);


    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "DocumentNotFound removing row: GrainId={GrainId} ReminderName={ReminderName} ETag={ETag}")]
    internal static partial void LogTraceDocumentNotFoundRemovingRow(this ILogger logger, GrainId grainId, string reminderName, string eTag);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "ETagMismatch removing row: GrainId={GrainId} ReminderName={ReminderName} ETag={ETag} StoredETag={StoredETag}")]
    internal static partial void LogTraceETagMismatchRemovingRow(this ILogger logger, GrainId grainId, string reminderName, string eTag, string storedETag);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Error removing row: GrainId={GrainId} ReminderName={ReminderName} ETag={ETag}")]
    internal static partial void LogErrorRemovingRow(this ILogger logger, Exception ex, GrainId grainId, string reminderName, string eTag);

    #endregion
}