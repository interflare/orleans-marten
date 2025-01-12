using Orleans.Providers.Marten.Reminders.Documents;
using Riok.Mapperly.Abstractions;

namespace Orleans.Providers.Marten.Reminders.Common.Mappers;

[Mapper]
[UseStaticMapper(typeof(SharedMappers))]
internal static partial class ReminderMappers
{
    [MapProperty(nameof(OrleansReminder.Version), nameof(ReminderEntry.ETag))]
    internal static partial ReminderEntry MapToNative(this OrleansReminder reminder);
}