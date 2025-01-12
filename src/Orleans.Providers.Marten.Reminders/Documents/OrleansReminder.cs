using Marten.Metadata;
using Orleans.Configuration;

namespace Orleans.Providers.Marten.Reminders.Documents;

/// <summary>
/// Represents a reminder for a grain.
/// </summary>
/// <seealso cref="ReminderEntry"/>
public record OrleansReminder : IVersioned
{
    /// <summary>
    /// The identifier of the document.
    /// </summary>
    /// <example><see cref="ServiceId"/>-<see cref="GrainId"/>-<see cref="ReminderName"/></example>
    public string Id { get; init; } = null!;

    /// <summary>
    /// The identifier of the service which this reminder is stored for.
    /// </summary>
    /// <seealso cref="ClusterOptions.ServiceId"/>
    public string ServiceId { get; init; } = null!;

    /// <summary>
    /// The identifier of the cluster which this reminder was created by.
    /// </summary>
    /// <remarks>
    /// This is used only for diagnostics.
    /// </remarks>
    public string ClusterId { get; init; } = null!;

    /// <summary>
    /// The name of the reminder.
    /// </summary>
    public string ReminderName { get; init; } = null!;

    /// <summary>
    /// The identifier of the grain from Orleans.
    /// </summary>
    public string GrainId { get; init; } = null!;

    /// <summary>
    /// The uniform hash code of the grain.
    /// </summary>
    /// <remarks>
    /// Used for querying ranges of reminders.
    /// </remarks>
    /// <seealso cref="GrainId.GetUniformHashCode()"/>
    public uint GrainHash { get; init; }

    /// <summary>
    /// The time when the reminder was supposed to tick-in the for first time.
    /// </summary>
    public DateTimeOffset StartAt { get; init; }

    /// <summary>
    /// The time period for the reminder.
    /// </summary>
    public TimeSpan Period { get; init; }

    /// <summary>
    /// The version of this provider.
    /// </summary>
    public int ProviderVersion { get; init; }

    /// <summary>
    /// The version of the document, managed by Marten.
    /// </summary>
    /// <remarks>
    /// This value is used as the <see cref="ReminderEntry.ETag"/> for optimistic concurrency.
    /// </remarks>
    public Guid Version { get; set; }
}