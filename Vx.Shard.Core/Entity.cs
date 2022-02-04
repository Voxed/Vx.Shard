// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Vx.Shard.Core.Specs")]

namespace Vx.Shard.Core;

/// <summary>
/// An entity in the game world.
/// </summary>
public readonly struct Entity
{
    private readonly ComponentStore _store;
    private readonly ComponentRegistry _componentRegistry;

    /// <summary>
    /// The numerical id of the entity.
    /// </summary>
    public readonly int Id;

    /// <summary>
    /// Create a new entity view from an entity id and the component store it exists in.
    /// </summary>
    /// <param name="id">The entity id.</param>
    /// <param name="store">The component store of the entity.</param>
    /// <param name="componentRegistry">The component registry to use.</param>
    internal Entity(int id, ComponentStore store, ComponentRegistry componentRegistry)
    {
        Id = id;
        _store = store;
        _componentRegistry = componentRegistry;
    }

    /// <summary>
    /// Add a component to the entity.
    /// If the component already exists on the entity, this does nothing.
    /// </summary>
    /// <param name="component">The component to add.</param>
    /// <typeparam name="T">The type of the component</typeparam>
    public Entity AddComponent<T>(T component) where T : IComponent
    {
        var componentId = _componentRegistry.GetComponentId<T>();
        _store.AddComponent(componentId, Id, component);
        return this;
    }

    /// <summary>
    /// Remove a component from the entity.
    /// If the component doesn't exist on the entity, this does nothing.
    /// </summary>
    /// <typeparam name="T">The type of the component to remove.</typeparam>
    public Entity RemoveComponent<T>() where T : IComponent
    {
        var componentId = _componentRegistry.GetComponentId<T>();
        _store.RemoveComponent(componentId, Id);
        return this;
    }

    /// <summary>
    /// Get a component on the entity.
    /// If the component does not exist on the entity, this returns null.
    /// </summary>
    /// <typeparam name="T">The type of the component to get.</typeparam>
    /// <returns>The component if it exists, otherwise null.</returns>
    public T? GetComponent<T>() where T : IComponent
    {
        var componentId = _componentRegistry.GetComponentId<T>();
        return (T?) _store.GetComponent(componentId, Id);
    }
}