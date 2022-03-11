using Vx.Shard.Common;
using Vx.Shard.Core;
using Vx.Shard.Graphics;

namespace Vx.Shard.GalaxyOne;

public class SystemSpatial : ISystem
{
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        componentRegistry.Register<ComponentPosition>();
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        messageBusListenerBuilder.AddCallback<MessageUpdate>(Update);
    }

    public void Initialize(World world)
    { }

    private void Update(World world, MessageUpdate msg)
    {
        // Move all renderer components to the position component location each tick.
        foreach (var entity in world.GetEntitiesWith<ComponentRenderer>().With<ComponentPosition>())
        {
            entity.GetComponent<ComponentRenderer>()!.DrawableContainer.Position =
                entity.GetComponent<ComponentPosition>()!.Position;
        }
    }
}