// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

namespace Vx.Shard.Core;

/// <summary>
/// The IComponentStoreListener describes the interface used to listen for component store changes.
/// </summary>
internal interface IComponentStoreListener
{
    /// <summary>
    /// This method is invoked after a component is added to an entity.
    /// </summary>
    /// <param name="entityId">The id of the entity which the component was added to.</param>
    /// <typeparam name="T">The type of the component which was added.</typeparam>
    public void OnCreation<T>(int entityId) where T : IComponent;

    /// <summary>
    /// This method is invoked before a component is removed from an entity.
    /// </summary>
    /// <param name="entityId">The id of the entity which the component will be removed from.</param>
    /// <typeparam name="T">The type of the component which is going to be removed.</typeparam>
    public void OnDestruction<T>(int entityId) where T : IComponent;
}

/// <summary>
/// The ComponentStore models the component and entity storage of a world. 
/// </summary>
internal class ComponentStore
{
    private readonly Dictionary<Type, Dictionary<int, IComponent>> components = new();
    private int nextEntityId;
    private IComponentStoreListener? listener;

    /// <summary>
    /// Set the listener of the component store.
    /// This replaces the current listener, so there can only be one listener.
    /// </summary>
    /// <param name="newListener">The new listener to use.</param>
    internal void SetListener(IComponentStoreListener newListener)
    {
        listener = newListener;
    }

    /// <summary>
    /// Create an entity and return it's id.
    /// </summary>
    /// <returns>The id of the newly created entity.</returns>
    internal int CreateEntity()
    {
        return nextEntityId++;
    }

    /// <summary>
    /// Add a component to an entity.
    /// If a component of the same type already exists on the entity, it won't be added.
    /// </summary>
    /// <param name="entityId">The entity to add the component to.</param>
    /// <param name="component">The component to add.</param>
    /// <typeparam name="T">The type of the component to add.</typeparam>
    internal void AddComponent<T>(int entityId, T component) where T : IComponent
    {
        if (!components.ContainsKey(typeof(T)))
            components.Add(typeof(T), new Dictionary<int, IComponent>());
        var entityComponentStore = components[typeof(T)];
        if (entityComponentStore.ContainsKey(entityId)) return;
        entityComponentStore.Add(entityId, component);
        listener?.OnCreation<T>(entityId);
    }

    /// <summary>
    /// Remove a component from an entity.
    /// If the component does not exist on the entity, nothing will happen.
    /// </summary>
    /// <param name="entityId">The entity to remove the component from.</param>
    /// <typeparam name="T">The type of the component to remove.</typeparam>
    internal void RemoveComponent<T>(int entityId) where T : IComponent
    {
        if (!components.ContainsKey(typeof(T))) return;
        var entityComponentStore = components[typeof(T)];
        if (!entityComponentStore.ContainsKey(entityId)) return;
        listener?.OnDestruction<T>(entityId);
        entityComponentStore.Remove(entityId);
    }

    /// <summary>
    /// Get a component from an entity.
    /// If the component does not exist on the entity, null will be returned.
    /// </summary>
    /// <param name="entityId">The entity to get the component from.</param>
    /// <typeparam name="T">The type of the component to get.</typeparam>
    /// <returns>The component, null if the component doesn't exist.</returns>
    internal T? GetComponent<T>(int entityId) where T : IComponent
    {
        if (!components.ContainsKey(typeof(T))) return default;
        var entityComponentStore = components[typeof(T)];
        if (entityComponentStore.ContainsKey(entityId))
        {
            return (T) entityComponentStore[entityId];
        }

        return default;
    }

    /// <summary>
    /// Get all entities with a specific component from storage.
    /// Because of the internal structure, this is very fast.
    /// </summary>
    /// <typeparam name="T">The type of the component to query for.</typeparam>
    /// <returns>The entities which have the specified component.</returns>
    internal List<int> GetEntitiesWith<T>() where T : IComponent
    {
        return components.ContainsKey(typeof(T)) ? components[typeof(T)].Keys.ToList() : new List<int>();
    }
}