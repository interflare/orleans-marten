using System.Diagnostics.CodeAnalysis;

namespace Orleans.Providers.Marten.Persistence;

[ExcludeFromCodeCoverage]
internal static class Constants
{
    /// <summary>
    /// The data version of this provider.
    /// </summary>
    internal const int ProviderVersion = 1;
}