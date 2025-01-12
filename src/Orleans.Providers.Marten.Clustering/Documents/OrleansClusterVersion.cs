using Marten.Metadata;
using Marten.Schema;

namespace Orleans.Providers.Marten.Clustering.Documents;

/// <summary>
/// Represents the version of the cluster across all silos.
/// </summary>
/// <seealso cref="TableVersion"/>
[UseOptimisticConcurrency]
public record OrleansClusterVersion : IVersioned
{
    /// <summary>
    /// The identifier of the document, which is the service and cluster id.
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// The version of this provider.
    /// </summary>
    public int ProviderVersion { get; init; }

    /// <summary>
    /// The table version.
    /// </summary>
    /// <remarks>
    /// A monotonically increasing number set by the Orleans runtime (or I would have used <see cref="IRevisioned"/>).
    /// </remarks>
    /// <seealso cref="TableVersion.Version"/>
    /// <seealso cref="MembershipTableData.Version"/>
    public int Revision { get; set; }

    /// <summary>
    /// The ETag for the document.
    /// </summary>
    /// <seealso cref="TableVersion.VersionEtag"/>
    public Guid Version { get; set; }
}