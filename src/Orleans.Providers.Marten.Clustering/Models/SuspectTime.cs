using Orleans.Runtime;

namespace Orleans.Providers.Marten.Clustering.Models;

/// <summary>
/// Represents a suspicion of a silo being dead by another silo.
/// </summary>
/// <remarks>
/// Used by the internal Orleans membership protocol for voting on the liveness of other silos.
/// </remarks>
/// <seealso cref="MembershipEntry.SuspectTimes"/>
/// <seealso cref="OrleansMembershipEntry.SuspectTimes"/>
public sealed record SuspectTime
{
    /// <summary>
    /// The silo which suspects the contextual silo is dead.
    /// </summary>
    public SiloAddress Silo { get; init; } = null!;

    /// <summary>
    /// The timestamp of when the suspecting <see cref="Silo"/> 
    /// </summary>
    public DateTimeOffset Timestamp { get; init; }
}