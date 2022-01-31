// See https://aka.ms/new-console-template for more information
using Vx.Shard.Example;
using Vx.Shard.Core;
using Vx.Shard.SDL;
using Vx.Shard.Graphics;
using Vx.Shard.Common;

Console.WriteLine(Directory.GetCurrentDirectory());

var drawableSystem = new SystemGraphics();
drawableSystem.RegisterDrawable<ClientTestComponent>();

new EngineBuilder()
.AddSystem(drawableSystem)
.AddSystem(new SystemSdl())
.AddSystem(new SystemSdlRenderer())
.AddSystem(new TestSystem())
.AddSystem(new SystemMainLoop())
.Build();

Console.WriteLine("Hello, World!");