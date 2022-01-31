// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

namespace Vx.Shard.Core;

/// <summary>
/// The Engine object allows for some external influences over the game world.
/// Specifically transmitting to the message bus.
/// </summary>
public class Engine
{
    private readonly World world;

    /// <summary>
    /// Constructs an engine from the chosen systems.
    /// </summary>
    /// <param name="systems">The systems to use.</param>
    internal Engine(
        List<ISystem> systems)
    {
        MessageBusListenerBuilder messageBusListenerBuilder = new();
        ComponentStoreListenerBuilder componentStoreListenerBuilder = new();
        systems.ForEach(system => system.Configure(messageBusListenerBuilder, componentStoreListenerBuilder));
        world = new World();
        world.ComponentStore.SetListener(componentStoreListenerBuilder.Build(world));
        world.MessageBus.SetListener(messageBusListenerBuilder.Build(world));
        systems.ForEach(system => system.Initialize(world));
    }

    /// <summary>
    /// Send a message externally onto the world message bus.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <typeparam name="T">The type of the message.</typeparam>
    public void Send<T>(T message) where T : IMessage
    {
        world.Send(message);
    }
}