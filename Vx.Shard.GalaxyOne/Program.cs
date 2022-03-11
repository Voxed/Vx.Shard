using Vx.Shard.Common;
using Vx.Shard.Core;
using Vx.Shard.GalaxyOne;
using Vx.Shard.Graphics;
using Vx.Shard.Resources;
using Vx.Shard.Sdl;
using Vx.Shard.Window;

var engine = new EngineBuilder()
    // Add API abstractions, these systems register appropriate messages/component and set-up implementation
    // independent logic.
    .AddSystem(new SystemInput())
    .AddSystem(new SystemGraphics())
    .AddSystem(new SystemResources())
    
    // Add API implementations, this could be dynamic, choosing implementations based on OS.
    .AddSystem(new SystemSdl())
    .AddSystem(new SystemSdlGraphics())
    .AddSystem(new SystemSdlInput())
    
    // Game specific systems.
    .AddSystem(new SystemMovement())
    .AddSystem(new SystemSpatial())
    .AddSystem(new SystemPlayerControl())
    .AddSystem(new SystemShip())
    .AddSystem(new SystemCollision())
    .AddSystem(new SystemGalaxyOne())

    .AddSystem(new SystemMainLoop())
    .Build();

engine.Start();