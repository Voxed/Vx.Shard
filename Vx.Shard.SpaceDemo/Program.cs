// See https://aka.ms/new-console-template for more information

using Vx.Shard.Common;
using Vx.Shard.Core;
using Vx.Shard.Graphics;
using Vx.Shard.Resources;
using Vx.Shard.Sdl;
using Vx.Shard.SpaceDemo;
using Vx.Shard.Window;

var engine = new EngineBuilder()
    // API systems
    .AddSystem(new SystemGraphics())
    .AddSystem(new SystemResources())
    .AddSystem(new SystemInput())
    // SDL systems (API implementations)
    .AddSystem(new SystemSdl())
    .AddSystem(new SystemSdlRenderer())
    .AddSystem(new SystemSdlInput())
    // Game systems
    .AddSystem(new SystemPlayer())
    .AddSystem(new SystemWorld())
    // Main loop
    .AddSystem(new SystemMainLoop())
    .Build();

engine.Start();