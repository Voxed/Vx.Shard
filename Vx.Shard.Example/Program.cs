// See https://aka.ms/new-console-template for more information
using Vx.Shard.Example;
using Vx.Shard.Core;
using Vx.Shard.Sdl;
using Vx.Shard.Graphics;
using Vx.Shard.Common;

Console.WriteLine(Directory.GetCurrentDirectory());

var engine = new EngineBuilder()
.AddSystem(new SystemGraphics())
.AddSystem(new SystemSdl())
.AddSystem(new SystemSdlRenderer())
.AddSystem(new TestSystem())
.AddSystem(new SystemMainLoop())
.Build();

engine.Start();