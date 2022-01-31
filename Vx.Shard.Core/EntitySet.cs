// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

namespace Vx.Shard.Core;

using System.Collections;

/// <summary>
/// A set of entities in the game world.
/// </summary>
public class EntitySet : IEnumerable<Entity>
{
    private readonly List<int> entities;
    private readonly ComponentStore store;

    /// <summary>
    /// Construct a new entity set from a list of entity ids and the component store they exist in.
    /// </summary>
    /// <param name="entities">The entity ids.</param>
    /// <param name="store">The component store where the entities reside.</param>
    internal EntitySet(List<int> entities, ComponentStore store)
    {
        this.entities = entities;
        this.store = store;
    }

    /// <summary>
    /// Get an enumerator to enumerate over the entities in the set.
    /// </summary>
    /// <returns>The entity enumerator.</returns>
    public IEnumerator<Entity> GetEnumerator()
    {
        return entities.Select(t => new Entity(t, store)).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Filter out entities in the set who have a specified component.
    /// </summary>
    /// <typeparam name="T">The component type to query for.</typeparam>
    /// <returns>A new set with entities who have the specified component.s</returns>
    public EntitySet With<T>() where T : IComponent
    {
        return new EntitySet(store.GetEntitiesWith<T>().Intersect(entities).ToList(), store);
    }
}