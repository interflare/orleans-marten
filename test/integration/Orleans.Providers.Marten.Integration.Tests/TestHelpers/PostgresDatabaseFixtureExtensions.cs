using Marten;
using Orleans.Providers.Marten.Integration.Tests.Infrastructure.Fixtures;
using System.Diagnostics.CodeAnalysis;

namespace Orleans.Providers.Marten.Integration.Tests.TestHelpers;

[ExcludeFromCodeCoverage]
internal static class PostgresDatabaseFixtureExtensions
{
    internal static async Task WaitUntilCanConnect(this PostgresDatabaseFixture fixture, TimeSpan? timeout = null, TimeSpan? delayBetweenRetries = null)
    {
        using var timeoutCts = new CancellationTokenSource(timeout ?? TimeSpan.FromSeconds(60));
        var timeoutToken = timeoutCts.Token;

        try
        {
            while (!timeoutToken.IsCancellationRequested)
            {
                try
                {
                    DocumentStore.For(fixture.ConnectionString).Diagnostics.GetPostgresVersion();
                    return;
                }
                catch
                {
                    // Do nothing on error
                }

                await Task.Delay(delayBetweenRetries ?? TimeSpan.FromMilliseconds(500), timeoutToken);
            }
        }
        catch (OperationCanceledException)
        {
            if (timeoutCts.IsCancellationRequested)
                throw new TimeoutException("could not connect to the database before the timeout");

            throw;
        }
    }
}