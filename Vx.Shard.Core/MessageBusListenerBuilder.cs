// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

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
        private readonly Dictionary<Type, List<object>> _callbacks;

        public ConcreteMessageBusListener(World world, Dictionary<Type, List<object>> callbacks)
        {
            _world = world;
            _callbacks = callbacks;
        }

        public void OnMessage<T>(T message) where T : IMessage
        {
            if (_callbacks.ContainsKey(typeof(T)))
            {
                _callbacks[typeof(T)].ForEach(cb => { ((Callback<T>) cb)(_world, message); });
            }
        }
    }

    /// <summary>
    /// A callback used for message handling.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public delegate void Callback<in T>(World world, T message);

    private readonly Dictionary<Type, List<object>> _callbacks = new();

    /// <summary>
    /// Add a callback to the message bus listener for a specific message type.
    /// </summary>
    /// <param name="callback">The callback to add.</param>
    /// <typeparam name="T">The message type to invoke the callback for.</typeparam>
    /// <returns>Self to allow for method chaining.</returns>
    public MessageBusListenerBuilder AddCallback<T>(Callback<T> callback) where T : IMessage
    {
        if (!_callbacks.ContainsKey(typeof(T)))
        {
            _callbacks.Add(typeof(T), new List<object>());
        }

        _callbacks[typeof(T)].Add(callback);

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