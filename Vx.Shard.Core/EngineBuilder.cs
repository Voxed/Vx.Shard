// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Vx.Shard.Core.Specs")]
namespace Vx.Shard.Core;

/// <summary>
/// The EngineBuilder has methods for adding systems and building the final engine.
/// </summary>
public class EngineBuilder
{
    private readonly List<ISystem> _systems = new();

    /// <summary>
    /// Add a system to the engine.
    /// </summary>
    /// <param name="system">The engine to add.</param>
    /// <returns>Self in order to allow for method chaining.</returns>
    public EngineBuilder AddSystem(ISystem system)
    {
        _systems.Add(system);
        return this;
    }

    /// <summary>
    /// Builds the engine and configures all the systems.
    /// </summary>
    /// <returns>The built engine.</returns>
    public Engine Build()
    {
        return new Engine(_systems);
    }
}