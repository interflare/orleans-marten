using Orleans.Runtime;

namespace Orleans.Providers.Marten.Clustering.Models;

/// <inheritdoc cref="MembershipEntry"/>
public record OrleansMembershipEntry
{
    /// <inheritdoc cref="MembershipEntry.SiloName"/>
    public string SiloName { get; init; } = null!;

    /// <inheritdoc cref="MembershipEntry.HostName"/>
    public string HostName { get; init; } = null!;

    /// <inheritdoc cref="MembershipEntry.SiloAddress"/>
    public SiloAddress SiloAddress { get; init; } = null!;

    /// <inheritdoc cref="MembershipEntry.ProxyPort"/>
    public int? ProxyPort { get; init; }

    /// <inheritdoc cref="MembershipEntry.RoleName"/>
    public string? RoleName { get; init; }

    /// <inheritdoc cref="MembershipEntry.UpdateZone"/>
    public int? UpdateZone { get; init; }

    /// <inheritdoc cref="MembershipEntry.FaultZone"/>
    public int? FaultZone { get; init; }

    /// <inheritdoc cref="MembershipEntry.SuspectTimes"/>
    public List<SuspectTime> SuspectTimes { get; init; } = [];

    /// <inheritdoc cref="MembershipEntry.StartTime"/>
    public DateTimeOffset StartTime { get; init; }

    /// <inheritdoc cref="MembershipEntry.IAmAliveTime"/>
    // ReSharper disable once InconsistentNaming
    public DateTimeOffset IAmAliveTime { get; init; }

    /// <inheritdoc cref="MembershipEntry.Status"/>
    public SiloStatus Status { get; init; }
}