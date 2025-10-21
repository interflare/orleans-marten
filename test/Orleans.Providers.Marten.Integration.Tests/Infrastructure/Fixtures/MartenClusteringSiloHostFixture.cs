using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace Orleans.Providers.Marten.Integration.Tests.Infrastructure.Fixtures;

public sealed class MartenClusteringSiloHostFixture : PostgresBackedSiloHostFixture
{
    protected override void ConfigureOrleansBuilder(ISiloBuilder siloBuilder)
        => siloBuilder.UseMartenClustering()
            .AddMemoryGrainStorageAsDefault();

    protected override void ConfigureServices(IServiceCollection services)
        => services.AddMarten(options => options.Connection(DatabaseFixture.ConnectionString))
            .UseLightweightSessions();
}