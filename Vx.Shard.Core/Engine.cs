// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

namespace Vx.Shard.Core;

/// <summary>
/// The Engine object allows for some external influences over the game world.
/// Specifically transmitting to the message bus.
/// </summary>
public class Engine
{
    private readonly World _world;
    private readonly List<ISystem> _systems;
    private bool _started = false;

    /// <summary>
    /// Constructs an engine from the chosen systems.
    /// </summary>
    /// <param name="systems">The systems to use.</param>
    internal Engine(
        List<ISystem> systems)
    {
        _systems = systems;
        MessageBusListenerBuilder messageBusListenerBuilder = new();
        ComponentStoreListenerBuilder componentStoreListenerBuilder = new();
        _systems.ForEach(system => system.Configure(messageBusListenerBuilder, componentStoreListenerBuilder));
        _world = new World();
        _world.ComponentStore.SetListener(componentStoreListenerBuilder.Build(_world));
        _world.MessageBus.SetListener(messageBusListenerBuilder.Build(_world));
    }

    /// <summary>
    /// Start the engine and initializes all the systems.
    /// </summary>
    public void Start()
    {
        _systems.ForEach(system => system.Initialize(_world));
        _started = true;
    }

    /// <summary>
    /// Send a message externally onto the world message bus.
    /// It's generally recommended to keep the engine self contained. So take care.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <typeparam name="T">The type of the message.</typeparam>
    public void Send<T>(T message) where T : IMessage
    {
        if (!_started)
            throw new InvalidOperationException("Tried to use the message bus before the engine was started");
        _world.Send(message);
    }
}