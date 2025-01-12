using Microsoft.Extensions.Logging;

namespace Orleans.Providers.Marten.Clustering.Common.Extensions;

internal static partial class LoggerExtensions
{
    #region Deleting

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Deleting: ClusterId={ClusterId}")]
    internal static partial void LogTraceDeleting(this ILogger logger, string clusterId);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Deleted: ClusterId={ClusterId}")]
    internal static partial void LogTraceDeleted(this ILogger logger, string clusterId);


    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Error deleting: ClusterId={ClusterId}")]
    internal static partial void LogErrorDeleting(this ILogger logger, Exception ex, string clusterId);

    #endregion

    #region Cleanup

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Cleaning: ClusterId={ClusterId} Before={Before}")]
    internal static partial void LogTraceCleaning(this ILogger logger, string clusterId, DateTimeOffset before);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Cleaned: ClusterId={ClusterId} Before={Before}")]
    internal static partial void LogTraceCleaned(this ILogger logger, string clusterId, DateTimeOffset before);


    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Error cleaning: ClusterId={ClusterId} Before={Before}")]
    internal static partial void LogErrorCleaning(this ILogger logger, Exception ex, string clusterId, DateTimeOffset before);

    #endregion

    #region ReadRow

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Reading row: ClusterId={ClusterId} SiloAddress={SiloAddress}")]
    internal static partial void LogTraceReadingRow(this ILogger logger, string clusterId, string siloAddress);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Read row: ClusterId={ClusterId} SiloAddress={SiloAddress}")]
    internal static partial void LogTraceReadRow(this ILogger logger, string clusterId, string siloAddress);


    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "DocumentNotFound reading row: ClusterId={ClusterId} SiloAddress={SiloAddress}")]
    internal static partial void LogTraceDocumentNotFoundReadingRow(this ILogger logger, string clusterId, string siloAddress);


    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Error reading row: ClusterId={ClusterId} SiloAddress={SiloAddress}")]
    internal static partial void LogErrorReadingRow(this ILogger logger, Exception ex, string clusterId, string siloAddress);

    #endregion

    #region ReadAllRows

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Reading all rows: ClusterId={ClusterId}")]
    internal static partial void LogTraceReadingAllRows(this ILogger logger, string clusterId);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Read all rows: ClusterId={ClusterId}")]
    internal static partial void LogTraceReadAllRows(this ILogger logger, string clusterId);


    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Error reading all rows: ClusterId={ClusterId}")]
    internal static partial void LogErrorReadingAllRows(this ILogger logger, Exception ex, string clusterId);

    #endregion

    #region InsertRow

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Inserting row: ClusterId={ClusterId} SiloAddress={SiloAddress} ETag={ETag}")]
    internal static partial void LogTraceInsertingRow(this ILogger logger, string clusterId, string siloAddress, string eTag);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Inserted rows: ClusterId={ClusterId} SiloAddress={SiloAddress} ETag={ETag}")]
    internal static partial void LogTraceInsertedRow(this ILogger logger, string clusterId, string siloAddress, string eTag);


    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "DocumentNotFound reading table version row: ClusterId={ClusterId} SiloAddress={SiloAddress} ETag={ETag}")]
    internal static partial void LogTraceDocumentNotFoundInsertingRow(this ILogger logger, string clusterId, string siloAddress, string eTag);


    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "DocumentAlreadyExists inserting row: ClusterId={ClusterId} SiloAddress={SiloAddress} ETag={ETag}")]
    internal static partial void LogErrorDocumentAlreadyExistsInsertingRow(this ILogger logger, Exception ex, string clusterId, string siloAddress, string eTag);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "ETagMismatch inserting row: ClusterId={ClusterId} SiloAddress={SiloAddress} ETag={ETag}")]
    internal static partial void LogErrorETagMismatchInsertingRow(this ILogger logger, Exception ex, string clusterId, string siloAddress, string eTag);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Error inserting row: ClusterId={ClusterId} SiloAddress={SiloAddress} ETag={ETag}")]
    internal static partial void LogErrorInsertingRow(this ILogger logger, Exception ex, string clusterId, string siloAddress, string eTag);

    #endregion

    #region UpdateRow

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Updating row: ClusterId={ClusterId} SiloAddress={SiloAddress} MemberETag={MemberETag} TableETag={TableETag}")]
    internal static partial void LogTraceUpdatingRow(this ILogger logger, string clusterId, string siloAddress, string memberETag, string tableETag);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Updated row: ClusterId={ClusterId} SiloAddress={SiloAddress} MemberETag={MemberETag} TableETag={TableETag}")]
    internal static partial void LogTraceUpdatedRow(this ILogger logger, string clusterId, string siloAddress, string memberETag, string tableETag);


    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "DocumentNotFound (table version) updating row: ClusterId={ClusterId} SiloAddress={SiloAddress} MemberETag={MemberETag} TableETag={TableETag}")]
    internal static partial void LogErrorTableVersionDocumentNotFoundUpdatingRow(this ILogger logger, string clusterId, string siloAddress, string memberETag, string tableETag);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "DocumentNotFound (member) updating row: ClusterId={ClusterId} SiloAddress={SiloAddress} MemberETag={MemberETag} TableETag={TableETag}")]
    internal static partial void LogErrorMemberDocumentNotFoundUpdatingRow(this ILogger logger, string clusterId, string siloAddress, string memberETag, string tableETag);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "ETagMismatch (table version) updating row: ClusterId={ClusterId} SiloAddress={SiloAddress} MemberETag={MemberETag} TableETag={TableETag}")]
    internal static partial void LogErrorTableVersionETagMismatchUpdatingRow(this ILogger logger, Exception ex, string clusterId, string siloAddress, string memberETag,
        string tableETag);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "ETagMismatch (member) updating row: ClusterId={ClusterId} SiloAddress={SiloAddress} MemberETag={MemberETag} TableETag={TableETag}")]
    internal static partial void LogErrorMemberETagMismatchUpdatingRow(this ILogger logger, Exception ex, string clusterId, string siloAddress, string memberETag,
        string tableETag);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Error updating row: ClusterId={ClusterId} SiloAddress={SiloAddress} MemberETag={MemberETag} TableETag={TableETag}")]
    internal static partial void LogErrorUpdatingRow(this ILogger logger, Exception ex, string clusterId, string siloAddress, string memberETag, string tableETag);

    #endregion

    #region UpdateIAmAlive

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Updating IAmAlive: ClusterId={ClusterId} SiloAddress={SiloAddress} LivenessTimestamp={LivenessTimestamp}")]
    internal static partial void LogTraceUpdatingIAmAlive(this ILogger logger, string clusterId, string siloAddress, DateTimeOffset livenessTimestamp);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Updated IAmAlive: ClusterId={ClusterId} SiloAddress={SiloAddress} LivenessTimestamp={LivenessTimestamp}")]
    internal static partial void LogTraceUpdatedIAmAlive(this ILogger logger, string clusterId, string siloAddress, DateTimeOffset livenessTimestamp);


    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Error updating IAmAlive: ClusterId={ClusterId} SiloAddress={SiloAddress} LivenessTimestamp={LivenessTimestamp}")]
    internal static partial void LogErrorUpdatingIAmAlive(this ILogger logger, Exception ex, string clusterId, string siloAddress, DateTimeOffset livenessTimestamp);

    #endregion

    #region GetGateways

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Listing gateways: ClusterId={ClusterId}")]
    internal static partial void LogTraceListingGateways(this ILogger logger, string clusterId);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Listed gateways: ClusterId={ClusterId}")]
    internal static partial void LogTraceListedGateways(this ILogger logger, string clusterId);


    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Error listing gateways: ClusterId={ClusterId}")]
    internal static partial void LogErrorListingGateways(this ILogger logger, Exception ex, string clusterId);

    #endregion


    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "DocumentNotFound reading cluster version: ClusterId={ClusterId}")]
    internal static partial void LogTraceDocumentNotFoundReadingClusterVersion(this ILogger logger, string clusterId);
}