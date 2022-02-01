// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Vx.Shard.Core.Specs")]

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

        private readonly Dictionary<int, List<(Action<World, Entity, IComponent>?, Action<World, Entity, IComponent>?)>>
            _callbacks;

        private readonly ComponentRegistry _componentRegistry;

        public ComponentStoreListener(World world,
            Dictionary<int, List<(Action<World, Entity, IComponent>?, Action<World, Entity, IComponent>?)>> callbacks,
            ComponentRegistry componentRegistry)
        {
            _world = world;
            _callbacks = callbacks;
            _componentRegistry = componentRegistry;
        }

        public void OnCreation(int componentId, int entityId, IComponent component)
        {
            if (_callbacks.ContainsKey(componentId))
            {
                _callbacks[componentId]
                    .ForEach(cb =>
                    {
                        cb.Item1?.Invoke(_world, new Entity(entityId, _world.ComponentStore, _componentRegistry),
                            component);
                    });
            }
        }

        public void OnDestruction(int componentId, int entityId, IComponent component)
        {
            if (_callbacks.ContainsKey(componentId))
            {
                _callbacks[componentId]
                    .ForEach(cb =>
                    {
                        cb.Item2?.Invoke(_world, new Entity(entityId, _world.ComponentStore, _componentRegistry),
                            component);
                    });
            }
        }
    }

    /// <summary>
    /// A callback used for component creation and destruction.
    /// </summary>
    /// <param name="world">The world where the entity resides.</param>
    /// <param name="entity">The entity which the component was added/removed from.</param>
    public delegate void Callback<in T>(World world, Entity entity, T component);

    private readonly Dictionary<int, List<(Action<World, Entity, IComponent>?, Action<World, Entity, IComponent>?)>>
        _callbacks = new();

    private readonly ComponentRegistry _componentRegistry;

    public ComponentStoreListenerBuilder(ComponentRegistry componentRegistry)
    {
        _componentRegistry = componentRegistry;
    }

    private void AddCallback(int componentId,
        Action<World, Entity, IComponent>? creationCallback, Action<World, Entity, IComponent>? destructionCallback)
    {
        if (componentId == 0)
            return;

        if (!_callbacks.ContainsKey(componentId))
        {
            _callbacks.Add(componentId,
                new List<(Action<World, Entity, IComponent>?, Action<World, Entity, IComponent>?)>());
        }

        _callbacks[componentId].Add((creationCallback, destructionCallback));
    }

    /// <summary>
    /// Add a creation and a destruction callbacks to the component store listener for a specific component type.
    /// </summary>
    /// <param name="callback">The callbacks to add, the first item is the creation callback, the second is the
    /// destruction callback.</param>
    /// <typeparam name="T">The component type to invoke the callback for.</typeparam>
    /// <returns>Self to allow for method chaining.</returns>
    public ComponentStoreListenerBuilder AddCallback<T>(Callback<T>? creationCallback = null,
        Callback<T>? destructionCallback = null)
        where T : IComponent
    {
        var id = _componentRegistry.GetComponentId<T>();
        AddCallback(id,
            creationCallback != null
                ? (world, entity, component) => { creationCallback(world, entity, (T) component); }
                : null,
            destructionCallback != null
                ? (world, entity, component) => { destructionCallback(world, entity, (T) component); }
                : null);
        Console.WriteLine("    * Configured callback for component {0} as {1}", id, typeof(T).Name);
        return this;
    }

    public ComponentStoreListenerBuilder AddSubtypeCallback<T>(Callback<T>? creationCallback = null,
        Callback<T>? destructionCallback = null)
    {
        _componentRegistry.GetSubtypeComponentIds<T>().ToList().ForEach(id =>
        {
            AddCallback(id,
                creationCallback != null
                    ? (world, entity, component) => { creationCallback(world, entity, (T) component); }
                    : null,
                destructionCallback != null
                    ? (world, entity, component) => { destructionCallback(world, entity, (T) component); }
                    : null);
            Console.WriteLine("    * Configured callback for component {0} as {1}", id, typeof(T).Name);
        });
        return this;
    }

    /// <summary>
    /// Build an IComponentStoreListener which invokes the configured callbacks.
    /// </summary>
    /// <param name="world">The world which the IComponentStoreListener will be used for.</param>
    /// <returns>The new IComponentStoreListener.</returns>
    internal IComponentStoreListener Build(World world)
    {
        return new ComponentStoreListener(world, _callbacks, _componentRegistry);
    }
}