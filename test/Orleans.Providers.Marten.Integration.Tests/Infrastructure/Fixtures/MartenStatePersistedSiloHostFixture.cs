using Marten;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Orleans.Providers.Marten.Integration.Tests.Infrastructure.Fixtures;

[ExcludeFromCodeCoverage]
public sealed class MartenStatePersistedSiloHostFixture : PostgresBackedSiloHostFixture
{
    protected override void ConfigureOrleansBuilder(ISiloBuilder siloBuilder)
        => siloBuilder.UseLocalhostClustering()
            .AddMartenGrainStorageAsDefault();

    protected override void ConfigureServices(IServiceCollection services)
        => services.AddMarten(options => options.Connection(DatabaseFixture.ConnectionString))
            .UseLightweightSessions();
}