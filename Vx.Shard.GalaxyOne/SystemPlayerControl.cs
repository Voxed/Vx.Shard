using Vx.Shard.Common;
using Vx.Shard.Core;
using Vx.Shard.Window;

namespace Vx.Shard.GalaxyOne;

public class SystemPlayerControl : ISystem
{
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        componentRegistry.Register<ComponentPlayerController>();
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        messageBusListenerBuilder.AddCallback<MessageUpdate>(Update);
    }

    public void Initialize(World world)
    {
        
    }

    private void Update(World world, MessageUpdate msg)
    {
        foreach (var entity in world.GetEntitiesWith<ComponentPlayerController>().With<ComponentVelocity>())
        {
            entity.GetComponent<ComponentVelocity>()!.Velocity =
                (world.GetSingletonComponent<ComponentMouse>()!.Position -
                 entity.GetComponent<ComponentPosition>()!.Position) / 8.0f;
        }
        
        
    }
}