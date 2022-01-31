// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

namespace Vx.Shard.Core;

/// <summary>
/// The ComponentStoreListenerBuilder builds an IComponentStoreListener which calls the configured callbacks.
/// </summary>
public class ComponentStoreListenerBuilder
{
    // This private implementation of the IComponentStoreListener invokes the callbacks passed in it's constructor based
    // on the type of component which was added/removed.
    private class ComponentStoreListener : IComponentStoreListener
    {
        private readonly World _world;
        private readonly Dictionary<Type, List<(Callback, Callback)>> _callbacks;

        public ComponentStoreListener(World world, Dictionary<Type, List<(Callback, Callback)>> callbacks)
        {
            _world = world;
            _callbacks = callbacks;
        }

        public void OnCreation<T>(int entityId) where T : IComponent
        {
            if (_callbacks.ContainsKey(typeof(T)))
            {
                _callbacks[typeof(T)].ForEach(cb => { cb.Item1(_world, new Entity(entityId, _world.ComponentStore)); });
            }
        }

        public void OnDestruction<T>(int entityId) where T : IComponent
        {
            if (_callbacks.ContainsKey(typeof(T)))
            {
                _callbacks[typeof(T)].ForEach(cb => { cb.Item2(_world, new Entity(entityId, _world.ComponentStore)); });
            }
        }
    }

    /// <summary>
    /// A callback used for component creation and destruction.
    /// </summary>
    /// <param name="world">The world where the entity resides.</param>
    /// <param name="entity">The entity which the component was added/removed from.</param>
    public delegate void Callback(World world, Entity entity);

    private readonly Dictionary<Type, List<(Callback, Callback)>> _callbacks
        = new();

    /// <summary>
    /// Add a creation and a destruction callbacks to the component store listener for a specific component type.
    /// </summary>
    /// <param name="callback">The callbacks to add, the first item is the creation callback, the second is the
    /// destruction callback.</param>
    /// <typeparam name="T">The component type to invoke the callback for.</typeparam>
    /// <returns>Self to allow for method chaining.</returns>
    public ComponentStoreListenerBuilder AddCallback<T>((Callback, Callback) callback) where T : IComponent
    {
        if (!_callbacks.ContainsKey(typeof(T)))
        {
            _callbacks.Add(typeof(T), new List<(Callback, Callback)>());
        }

        _callbacks[typeof(T)].Add(callback);
        return this;
    }

    /// <summary>
    /// Build an IComponentStoreListener which invokes the configured callbacks.
    /// </summary>
    /// <param name="world">The world which the IComponentStoreListener will be used for.</param>
    /// <returns>The new IComponentStoreListener.</returns>
    internal IComponentStoreListener Build(World world)
    {
        return new ComponentStoreListener(world, _callbacks);
    }
}