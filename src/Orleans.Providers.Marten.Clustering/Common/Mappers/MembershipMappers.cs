using Orleans.Providers.Marten.Clustering.Models;
using Orleans.Runtime;
using Riok.Mapperly.Abstractions;

namespace Orleans.Providers.Marten.Clustering.Common.Mappers;

[Mapper]
[UseStaticMapper(typeof(SharedMappers))]
internal static partial class MembershipMappers
{
    [UserMapping]
    private static SuspectTime MapToModel(Tuple<SiloAddress?, DateTime>? suspectTuple) =>
        new() { Silo = suspectTuple?.Item1 ?? SiloAddress.Zero, Timestamp = suspectTuple?.Item2 ?? DateTime.MinValue };

    [UserMapping]
    private static Tuple<SiloAddress?, DateTime> MapToNative(SuspectTime suspectTime) => new(suspectTime.Silo, suspectTime.Timestamp.DateTime);

    internal static partial OrleansMembershipEntry MapToModel(this MembershipEntry membership);
    internal static partial MembershipEntry MapToNative(this OrleansMembershipEntry membership);
}