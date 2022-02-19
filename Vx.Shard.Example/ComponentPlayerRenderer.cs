using Vx.Shard.Core;
using Vx.Shard.Graphics;
using Vx.Shard.Resources;

namespace Vx.Shard.Example;

public record ComponentPlayerRenderer : IDrawableComponent, IResourceComponent, IComponent
{
    public record ResourcePack
    {
        public readonly ResourceReference ShipTopDown = new ResourceReference
        {
            Path = "ship.png",
            Type = typeof(ResourceTexture)
        };
    }

    public readonly ResourcePack Resources = new();

    public ResourceReference[] GetResources()
    {
        return new[] {this.Resources.ShipTopDown};
    }

    public IDrawable Drawable { get; init; }
}