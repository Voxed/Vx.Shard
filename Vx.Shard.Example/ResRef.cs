using Vx.Shard.Core;
using Vx.Shard.Resources;

namespace Vx.Shard.Example;

public record ResRef : IComponent, IResourceComponent
{
    public ResourceReference Test { get; set; }
    
    public ResourceReference[] GetResources()
    {
        return new[] { Test };
    }
}