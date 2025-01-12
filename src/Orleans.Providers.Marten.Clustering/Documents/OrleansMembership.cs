using Marten.Metadata;
using Marten.Schema;
using Orleans.Configuration;
using Orleans.Providers.Marten.Clustering.Models;
using Orleans.Runtime;

namespace Orleans.Providers.Marten.Clustering.Documents;

/// <summary>
/// Represents the state of a silo within a cluster.
/// </summary>
/// <seealso cref="MembershipEntry"/>
[UseOptimisticConcurrency]
public record OrleansMembership : IVersioned
{
    /// <summary>
    /// The identifier of the document.
    /// </summary>
    /// <example><see cref="ServiceId"/>/<see cref="ClusterId"/>-<see cref="SiloAddress"/></example>
    public string Id { get; init; } = null!;

    /// <summary>
    /// The version of this provider.
    /// </summary>
    public int ProviderVersion { get; init; }

    /// <summary>
    /// The identifier of the service which this silo is a part of.
    /// </summary>
    /// <seealso cref="ClusterOptions.ServiceId"/>
    public string ServiceId { get; init; } = null!;

    /// <summary>
    /// The identifier of the cluster this silo is a part of.
    /// </summary>
    /// <seealso cref="ClusterOptions.ClusterId"/>
    public string ClusterId { get; init; } = null!;

    /// <summary>
    /// The ETag for the document.
    /// </summary>
    public Guid Version { get; set; }

    /// <summary>
    /// The actual data of the membership.
    /// </summary>
    public OrleansMembershipEntry Entry { get; init; } = null!;
}