// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

namespace Vx.Shard.Core;

/// <summary>
/// An entity in the game world.
/// </summary>
public class Entity
{
    private readonly ComponentStore store;

    /// <summary>
    /// The numerical id of the entity.
    /// </summary>
    public readonly int Id;

    /// <summary>
    /// Create a new entity view from an entity id and the component store it exists in.
    /// </summary>
    /// <param name="id">The entity id.</param>
    /// <param name="store">The component store of the entity.</param>
    internal Entity(int id, ComponentStore store)
    {
        Id = id;
        this.store = store;
    }

    /// <summary>
    /// Add a component to the entity.
    /// If the component already exists on the entity, this does nothing.
    /// </summary>
    /// <param name="component">The component to add.</param>
    /// <typeparam name="T">The type of the component</typeparam>
    public void AddComponent<T>(T component) where T : IComponent
    {
        store.AddComponent(Id, component);
    }

    /// <summary>
    /// Remove a component from the entity.
    /// If the component doesn't exist on the entity, this does nothing.
    /// </summary>
    /// <typeparam name="T">The type of the component to remove.</typeparam>
    public void RemoveComponent<T>() where T : IComponent
    {
        store.RemoveComponent<T>(Id);
    }

    /// <summary>
    /// Get a component on the entity.
    /// If the component does not exist on the entity, this returns null.
    /// </summary>
    /// <typeparam name="T">The type of the component to get.</typeparam>
    /// <returns>The component if it exists, otherwise null.</returns>
    public T? GetComponent<T>() where T : IComponent
    {
        return store.GetComponent<T>(Id);
    }
}