using Marten.Metadata;
using Marten.Schema;

namespace Orleans.Providers.Marten.Persistence.Documents;

/// <summary>
/// Represents the state data for a grain, as well as some provider metadata.
/// </summary>
[UseOptimisticConcurrency]
public sealed record OrleansState : IVersioned
{
    /// <summary>
    /// The identifier of the document.
    /// </summary>
    /// <example><see cref="StateName"/>-<see cref="GrainId"/></example>
    public string Id { get; init; } = null!;

    /// <summary>
    /// The name of the Orleans state.
    /// </summary>
    public string StateName { get; init; } = null!;

    /// <summary>
    /// The identifier of the grain from Orleans.
    /// </summary>
    public string GrainId { get; init; } = null!;

    /// <summary>
    /// The version of this provider.
    /// </summary>
    public int ProviderVersion { get; init; }

    /// <summary>
    /// The version of the document, managed by Marten.
    /// </summary>
    /// <remarks>
    /// This value is used as the <see cref="IGrainState{T}.ETag"/> for optimistic concurrency.
    /// </remarks>
    public Guid Version { get; set; }

    /// <summary>
    /// The actual state data, serialized as JSON.
    /// </summary>
    public string? Data { get; init; }
}