// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Vx.Shard.Core.Specs")]

namespace Vx.Shard.Core;

/// <summary>
/// The MessageBusListenerBuilder builds an IMessageBusListener which calls the configured callbacks.
/// </summary>
public class MessageBusListenerBuilder
{
    // This private implementation of the IMessageBusListener invokes the callbacks passed in it's constructor based
    // on the type of message which was sent.
    private class ConcreteMessageBusListener : IMessageBusListener
    {
        private readonly World _world;
        private readonly Dictionary<int, List<Action<World, IMessage>>> _callbacks;

        public ConcreteMessageBusListener(World world, Dictionary<int, List<Action<World, IMessage>>> callbacks)
        {
            _world = world;
            _callbacks = callbacks;
        }

        public void OnMessage(int messageId, IMessage message)
        {
            if (_callbacks.ContainsKey(messageId))
            {
                _callbacks[messageId].ForEach(cb => { cb(_world, message); });
            }
        }
    }

    /// <summary>
    /// A callback used for message handling.
    /// </summary>
    /// <typeparam name="T">The type of the message.</typeparam>
    public delegate void Callback<in T>(World world, T message);

    private readonly Dictionary<int, List<Action<World, IMessage>>> _callbacks = new();
    private readonly MessageRegistry _messageRegistry;

    /// <summary>
    /// Construct a new message bus listener builder.
    /// </summary>
    /// <param name="messageRegistry">The message registry to use.</param>
    public MessageBusListenerBuilder(MessageRegistry messageRegistry)
    {
        _messageRegistry = messageRegistry;
    }

    /// <summary>
    /// Add a callback to the message bus listener for a specific message type.
    /// </summary>
    /// <param name="callback">The callback to add.</param>
    /// <typeparam name="T">The message type to invoke the callback for.</typeparam>
    /// <returns>Self to allow for method chaining.</returns>
    public MessageBusListenerBuilder AddCallback<T>(Callback<T> callback) where T : IMessage
    {
        var messageId = _messageRegistry.GetMessageId<T>();

        if (messageId == 0)
            return this;

        if (!_callbacks.ContainsKey(messageId))
        {
            _callbacks.Add(messageId, new List<Action<World, IMessage>>());
        }

        _callbacks[messageId].Add((world, message) => callback(world, (T) message));

        return this;
    }

    /// <summary>
    /// Build an IMessageBusListener which invokes the configured callbacks.
    /// </summary>
    /// <param name="world">The world for which the IMessageBusListener will be used.</param>
    /// <returns>The new IMessageBusListener.</returns>
    internal IMessageBusListener Build(World world)
    {
        return new ConcreteMessageBusListener(world, _callbacks);
    }
}