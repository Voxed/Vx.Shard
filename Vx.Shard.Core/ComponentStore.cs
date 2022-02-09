// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Vx.Shard.Core.Specs")]

namespace Vx.Shard.Core;

/// <summary>
/// The IComponentStoreListener describes the interface used to listen for component store changes.
/// </summary>
internal interface IComponentStoreListener
{
    /// <summary>
    /// This method is invoked after a component is added to an entity.
    /// </summary>
    /// <param name="componentId">The id of the component added.</param>
    /// <param name="entityId">The id of the entity which the component was added to.</param>
    /// <param name="component">The data of the component going to be destroyed.</param>
    public void OnCreation(int componentId, int entityId, IComponent component);

    /// <summary>
    /// This method is invoked before a component is removed from an entity.
    /// </summary>
    /// <param name="componentId">The id of the component going to be destroyed.</param>
    /// <param name="entityId">The id of the entity which the component will be removed from.</param>
    /// <param name="component">The data of the component going to be destroyed.</param>
    public void OnDestruction(int componentId, int entityId, IComponent component);
}

/// <summary>
/// The ComponentStore models the component and entity storage of a world. 
/// </summary>
internal class ComponentStore
{
    private readonly Dictionary<int, SortedDictionary<int, IComponent>> _components = new();
    private int _nextEntityId;
    private IComponentStoreListener? _listener;

    /// <summary>
    /// Set the listener of the component store.
    /// This replaces the current listener, so there can only be one listener.
    /// </summary>
    /// <param name="newListener">The new listener to use.</param>
    internal void SetListener(IComponentStoreListener newListener)
    {
        _listener = newListener;
    }

    /// <summary>
    /// Create an entity and return it's id.
    /// </summary>
    /// <returns>The id of the newly created entity.</returns>
    internal int CreateEntity()
    {
        return _nextEntityId++;
    }

    /// <summary>
    /// Add a component to an entity.
    /// If a component of the same type already exists on the entity, it won't be added.
    /// </summary>
    /// <param name="componentId">The id of the component to add.</param>
    /// <param name="entityId">The entity to add the component to.</param>
    /// <param name="component">The component to add.</param>
    internal void AddComponent(int componentId, int entityId, IComponent component)
    {
        if (!_components.ContainsKey(componentId))
            _components.Add(componentId, new SortedDictionary<int, IComponent>());
        var entityComponentStore = _components[componentId];
        if (entityComponentStore.ContainsKey(entityId)) return;
        entityComponentStore.Add(entityId, component);
        _listener?.OnCreation(componentId, entityId, component);
    }

    /// <summary>
    /// Remove a component from an entity.
    /// If the component does not exist on the entity, nothing will happen.
    /// </summary>
    /// <param name="componentId">The id of the component to remove.</param>
    /// <param name="entityId">The entity to remove the component from.</param>
    internal void RemoveComponent(int componentId, int entityId)
    {
        if (!_components.ContainsKey(componentId)) return;
        var entityComponentStore = _components[componentId];
        if (!entityComponentStore.ContainsKey(entityId)) return;
        _listener?.OnDestruction(componentId, entityId, entityComponentStore[entityId]);
        entityComponentStore.Remove(entityId);
    }

    /// <summary>
    /// Get a component from an entity.
    /// If the component does not exist on the entity, null will be returned.
    /// </summary>
    /// <param name="componentId">The id of the component to get.</param>
    /// <param name="entityId">The entity to get the component from.</param>
    /// <returns>The component, null if the component doesn't exist.</returns>
    internal IComponent? GetComponent(int componentId, int entityId)
    {
        if (!_components.ContainsKey(componentId)) return default;
        var entityComponentStore = _components[componentId];
        return entityComponentStore.ContainsKey(entityId) ? entityComponentStore[entityId] : default;
    }

    internal IComponent? GetSingletonComponent(int componentId)
    {
        if (!_components.ContainsKey(componentId)) return default;
        return _components[componentId].Values.First();
    }

    /// <summary>
    /// Get all entities with a specific component from storage.
    /// Because of the internal structure, this is very fast.
    /// </summary>
    /// <param name="componentId">The id of the component to query for.</param>
    /// <returns>The entities which have the specified component.</returns>
    internal SortedSet<int>? GetEntitiesWith(int componentId)
    {
        return _components.ContainsKey(componentId) ? new SortedSet<int>(_components[componentId].Keys) : default;
    }
}