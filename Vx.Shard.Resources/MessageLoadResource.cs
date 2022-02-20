using Vx.Shard.Core;

namespace Vx.Shard.Resources;

public class MessageLoadResource : IMessage
{
    public ResourceInitializer Initializer { get; init; }

    public MessageLoadResource(ResourceInitializer initializer)
    {
        Initializer = initializer;
    }
}