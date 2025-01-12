using Orleans.Storage;

namespace Orleans.Providers.Marten.Persistence;

public sealed class MartenGrainStorageOptions : IStorageProviderSerializerOptions
{
    public IGrainStorageSerializer GrainStorageSerializer { get; set; } = null!;
}