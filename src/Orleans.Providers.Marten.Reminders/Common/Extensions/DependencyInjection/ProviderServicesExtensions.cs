// ReSharper disable CheckNamespace - this is the suggested namespace for dependency injection

using Marten;
using Orleans.Configuration;
using Orleans.Providers.Marten.Reminders;
using Orleans.Providers.Marten.Reminders.Documents;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class ProviderServicesExtensions
{
    /// <summary>
    /// Configures this silo to use Marten for reminders.
    /// </summary>
    /// <param name="services">The service collection being configured.</param>
    /// <remarks>
    /// <para>
    /// Note that only one reminder service provider can be configured as the default across the silo - the first registered wins.
    /// </para>
    /// <para>
    /// The <see cref="ClusterOptions"/> configured in the DI container are used to set the <see cref="ClusterOptions.ServiceId"/> for 'tenanting' the reminder storage, which is optional.
    /// </para>
    /// <para>
    /// Expects that Marten is already configured. This method will add its own document types to the Marten configuration - be sure to perform database migrations when this library is added for the first time, or updated.
    /// </para>
    /// </remarks>
    public static IServiceCollection UseMartenReminderService(this IServiceCollection services)
    {
        services.ConfigureMarten(options => options.ConfigurePersistence());

        services.AddReminders();
        services.AddSingleton<IReminderTable, MartenReminderTable>();

        return services;
    }

    /// <summary>
    /// Configures this silo to use Marten for reminders.
    /// </summary>
    /// <param name="builder">The silo being configured.</param>
    /// <remarks>
    /// <para>
    /// Note that only one reminder service provider can be configured as the default across the silo - the first registered wins.
    /// </para>
    /// <para>
    /// The <see cref="ClusterOptions"/> configured in the DI container are used to set the <see cref="ClusterOptions.ServiceId"/> for 'tenanting' the reminder storage, which is optional.
    /// </para>
    /// <para>
    /// Expects that Marten is already configured. This method will add its own document types to the Marten configuration - be sure to perform database migrations when this library is added for the first time, or updated.
    /// </para>
    /// </remarks>
    public static ISiloBuilder UseMartenReminderService(this ISiloBuilder builder)
        => builder.ConfigureServices(services => services.UseMartenReminderService());


    private static StoreOptions ConfigurePersistence(this StoreOptions options)
    {
        options.Schema.For<OrleansReminder>()
            .Index(r => r.GrainId)
            .Index(r => r.GrainHash);

        return options;
    }
}