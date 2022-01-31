namespace Vx.Shard.Core;

public class EngineBuilder
{
    private readonly List<ISystem> systems = new List<ISystem>();
    private readonly MessageBusListenerBuilder messageBusListenerBuilder = new MessageBusListenerBuilder();
    private readonly ComponentStoreListenerBuilder componentStoreListenerBuilder = new ComponentStoreListenerBuilder();

    public EngineBuilder AddSystem(ISystem system)
    {
        systems.Add(system);
        return this;
    }

    public Engine Build()
    {
        this.systems.ForEach(system => system.Configure(messageBusListenerBuilder, componentStoreListenerBuilder));
        Engine engine = new Engine(systems, messageBusListenerBuilder, componentStoreListenerBuilder);
        return engine;
    }
}