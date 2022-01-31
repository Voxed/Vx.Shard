// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

namespace Vx.Shard.Core;

using System.Collections;

/// <summary>
/// A set of entities in the game world.
/// </summary>
public class EntitySet : IEnumerable<Entity>
{
    private readonly List<int> _entities;
    private readonly ComponentStore _store;

    /// <summary>
    /// Construct a new entity set from a list of entity ids and the component store they exist in.
    /// </summary>
    /// <param name="entities">The entity ids.</param>
    /// <param name="store">The component store where the entities reside.</param>
    internal EntitySet(List<int> entities, ComponentStore store)
    {
        _entities = entities;
        _store = store;
    }

    /// <summary>
    /// Get an enumerator to enumerate over the entities in the set.
    /// </summary>
    /// <returns>The entity enumerator.</returns>
    public IEnumerator<Entity> GetEnumerator()
    {
        return new EntitySetEnumerator(_entities, _store);
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
        return new EntitySet(_store.GetEntitiesWith<T>().Intersect(_entities).ToList(), _store);
    }
}

/// <summary>
/// Enumerator of an EntitySet.
/// </summary>
public class EntitySetEnumerator : IEnumerator<Entity>
{
    private readonly ComponentStore _store;
    private readonly List<int> _entities;
    private int _position = -1;

    /// <summary>
    /// Construct a new EntitySetEnumerator from entities and the component store where they reside.
    /// </summary>
    /// <param name="list">The entities.</param>
    /// <param name="store">The component store.</param>
    internal EntitySetEnumerator(List<int> list, ComponentStore store)
    {
        _entities = list;
        _store = store;
    }

    // Below follows standard implemented enumerator methods.
    public bool MoveNext()
    {
        _position++;
        return (_position < _entities.Count);
    }

    public void Reset()
    {
        _position = -1;
    }
    
    object IEnumerator.Current => Current;

    public Entity Current => new(_entities[_position], _store);

    public void Dispose()
    {
    }
}