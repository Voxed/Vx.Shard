// See https://aka.ms/new-console-template for more information

using Vx.Shard.Example;
using Vx.Shard.Core;
using Vx.Shard.Sdl;
using Vx.Shard.Graphics;
using Vx.Shard.Common;
using Vx.Shard.Resources;
using Vx.Shard.Window;

Console.WriteLine(Directory.GetCurrentDirectory());

var engine = new EngineBuilder()
    .AddSystem(new SystemInput())
    .AddSystem(new SystemResources())
    .AddSystem(new SystemGraphics())
    .AddSystem(new SystemSdl())
    .AddSystem(new SystemSdlRenderer())
    .AddSystem(new SystemSdlInput())
    .AddSystem(new TestSystem())
    .AddSystem(new SystemMainLoop())
    .Build();

engine.Start();