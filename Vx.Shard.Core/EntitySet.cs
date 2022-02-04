// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Vx.Shard.Core.Specs")]

namespace Vx.Shard.Core;

using System.Collections;

/// <summary>
/// A set of entities in the game world.
/// </summary>
public readonly struct EntitySet : IEnumerable<Entity>
{
    private readonly List<SortedDictionary<int, IComponent>.KeyCollection> _entityCollections;
    private readonly ComponentStore _store;
    private readonly ComponentRegistry _componentRegistry;

    /// <summary>
    /// Construct a new entity set from a list of entity ids and the component store they exist in.
    /// </summary>
    /// <param name="entities">The entity ids.</param>
    /// <param name="store">The component store where the entities reside.</param>
    private EntitySet(List<SortedDictionary<int, IComponent>.KeyCollection> entityCollections, ComponentStore store,
        ComponentRegistry componentRegistry)
    {
        _entityCollections = entityCollections;
        _store = store;
        _componentRegistry = componentRegistry;
    }

    internal EntitySet(SortedDictionary<int, IComponent>.KeyCollection? entityCollections, ComponentStore store,
        ComponentRegistry componentRegistry)
    {
        _entityCollections =
            new List<SortedDictionary<int, IComponent>.KeyCollection>(entityCollections != null ? 1 : 0);
        if (entityCollections != null) _entityCollections.Add(entityCollections);
        _store = store;
        _componentRegistry = componentRegistry;
    }

    /// <summary>
    /// Get an enumerator to enumerate over the entities in the set.
    /// </summary>
    /// <returns>The entity enumerator.</returns>
    public EntitySetEnumerator GetEnumerator()
    {
        var enumerators =
            new List<SortedDictionary<int, IComponent>.KeyCollection.Enumerator>(_entityCollections.Count);
        if (_entityCollections.Count <= 0) return new EntitySetEnumerator(enumerators, _store, _componentRegistry);
        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var v in _entityCollections)
            enumerators.Add(v.GetEnumerator());
        return new EntitySetEnumerator(enumerators, _store, _componentRegistry);
    }

    IEnumerator<Entity> IEnumerable<Entity>.GetEnumerator()
    {
        return GetEnumerator();
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
        var componentId = _componentRegistry.GetComponentId<T>();
        var newCollection = _store.GetEntitiesWith(componentId);
        if (newCollection == null) return this;
        var newEntityCollections =
            new List<SortedDictionary<int, IComponent>.KeyCollection>(_entityCollections.Count + 1);
        foreach (var v in _entityCollections)
        {
            newEntityCollections.Add(v);
        }

        newEntityCollections.Add(newCollection);
        return new EntitySet(newEntityCollections, _store,
            _componentRegistry);
    }
}

/// <summary>
/// Enumerator of an EntitySet.
/// </summary>
public struct EntitySetEnumerator : IEnumerator<Entity>
{
    private readonly ComponentRegistry _componentRegistry;
    private readonly ComponentStore _store;
    private readonly SortedDictionary<int, IComponent>.KeyCollection.Enumerator[] _entities;
    private int _currValue = -1;
    private bool _done;

    /// <summary>
    /// Construct a new EntitySetEnumerator from entities and the component store where they reside.
    /// </summary>
    /// <param name="list">The entities.</param>
    /// <param name="store">The component store.</param>
    internal EntitySetEnumerator(List<SortedDictionary<int, IComponent>.KeyCollection.Enumerator> list,
        ComponentStore store, ComponentRegistry componentRegistry)
    {
        _entities = list.ToArray();
        _store = store;
        _componentRegistry = componentRegistry;
        _done = _entities.Length == 0;
    }

    private (int, int) AllSame(int pos = 0)
    {
        if (_entities.Length == pos + 1) return (_entities[pos].Current, pos);

        (int, int) allSame;
        if (_entities[pos].Current == (allSame = AllSame(pos + 1)).Item1)
            return (_entities[pos].Current, allSame.Item2);
        return _entities[pos].Current < _entities[allSame.Item2].Current ? (-1, pos) : (-1, allSame.Item2);
    }

    public bool MoveNext()
    {
        if (_done)
            return false;

        for (var i = 0; i < _entities.Length; i++)
        {
            if (!_entities[i].MoveNext())
            {
                _done = true;
                return false;
            }
        }

        (int, int) allSame;
        while ((allSame = AllSame()).Item1 == -1)
        {
            if (_entities[allSame.Item2].MoveNext()) continue;
            _done = true;
            return false;
        }

        _currValue = allSame.Item1;

        return true;
    }

    public void Reset()
    {
        throw new InvalidOperationException();
    }

    object IEnumerator.Current => Current;

    public Entity Current =>
        _done || _currValue == -1
            ? throw new InvalidOperationException()
            : new Entity(_currValue, _store, _componentRegistry);

    public void Dispose()
    {
    }
}