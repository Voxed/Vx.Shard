namespace Vx.Shard.Core;

public class Engine
{
    private readonly World world;

    internal Engine(List<ISystem> systems, MessageBusListenerBuilder messageBusListenerBuilder, ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        this.world = new World(systems, messageBusListenerBuilder, componentStoreListenerBuilder);

        systems.ForEach(system => system.Initialize(this.world));
    }

    public void Send<T>(T message) where T : IMessage
    {
        this.world.Send(message);
    }
}