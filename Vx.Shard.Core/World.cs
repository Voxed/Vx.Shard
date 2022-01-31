// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

namespace Vx.Shard.Core;

/// <summary>
/// Class World models the game world. It allows transmitting to the message-bus and operating on entities.
/// </summary>
public class World
{
    internal readonly MessageBus MessageBus = new();
    internal readonly ComponentStore ComponentStore = new();

    /// <summary>
    /// Send a message onto the world message bus.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <typeparam name="T">The type of the message to send.</typeparam>
    public void Send<T>(T message) where T : IMessage
    {
        MessageBus.Send(message);
    }

    /// <summary>
    /// Create an entity in the world.
    /// </summary>
    /// <returns>The entity created.</returns>
    public Entity CreateEntity()
    {
        return new Entity(ComponentStore.CreateEntity(), ComponentStore);
    }

    /// <summary>
    /// Get all entities with a specified component type.
    /// </summary>
    /// <typeparam name="T">The type of the component to query for.</typeparam>
    /// <returns>The entities with the requested component.</returns>
    public EntitySet GetEntitiesWith<T>() where T : IComponent
    {
        return new EntitySet(ComponentStore.GetEntitiesWith<T>(), ComponentStore);
    }
}