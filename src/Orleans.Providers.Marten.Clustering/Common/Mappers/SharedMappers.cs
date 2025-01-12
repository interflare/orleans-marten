using Riok.Mapperly.Abstractions;

namespace Orleans.Providers.Marten.Clustering.Common.Mappers;

[Mapper]
internal static partial class SharedMappers
{
    [UserMapping]
    internal static DateTimeOffset MapToDateTimeOffset(this DateTime dateTime) => dateTime;
    
    [UserMapping]
    internal static DateTime MapToDateTime(this DateTimeOffset dateTimeOffset) => dateTimeOffset.UtcDateTime;
}