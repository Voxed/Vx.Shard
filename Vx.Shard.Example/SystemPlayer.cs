using Vx.Shard.Core;

namespace Vx.Shard.Example;

public class SystemPlayer : ISystem
{
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        componentRegistry.Register<ComponentPlayerRenderer>();
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
    }

    public void Initialize(World world)
    {
        world.CreateEntity().AddComponent(new ComponentPlayerRenderer());
    }
}