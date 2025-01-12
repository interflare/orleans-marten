using Orleans.Runtime;

namespace Orleans.Providers.Marten.Clustering.Models;

public record OrleansMembershipEntry
{
    public string SiloName { get; init; } = null!;
    public string HostName { get; init; } = null!;
    public SiloAddress SiloAddress { get; init; } = null!;
    public int? ProxyPort { get; init; }
    public string? RoleName { get; init; }
    public int? UpdateZone { get; init; }
    public int? FaultZone { get; init; }
    public List<SuspectTime> SuspectTimes { get; init; } = [];

    public DateTimeOffset StartTime { get; init; }


    /// <summary>
    /// The last time the silo reported that it was alive.
    /// </summary>
    /// <remarks>
    /// For diagnostics and troubleshooting only.
    /// </remarks>
    // ReSharper disable once InconsistentNaming
    public DateTimeOffset IAmAliveTime { get; init; }

    public SiloStatus Status { get; init; }
}