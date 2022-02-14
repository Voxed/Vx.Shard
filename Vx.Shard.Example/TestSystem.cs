using Vx.Shard.Graphics;
using Vx.Shard.Resources;
using Vx.Shard.Window;

namespace Vx.Shard.Example;

using Core;
using Common;

public class TestSystem : ISystem
{
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        componentRegistry.Register<ResRef>();
        componentRegistry.Register<ClientTestComponent>();
        componentRegistry.Register<PositionComponent>();
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        messageBusListenerBuilder.AddCallback<MessageUpdate>(Update);
        messageBusListenerBuilder.AddCallback<MessageInputWindowClose>(Close);
        componentStoreListenerBuilder.AddCallback<PositionComponent>(
            (_, entity, component) =>
            {
                entity.AddComponent(new ResRef
                {   
                    Test = new ResourceReference
                    {
                        Path = "ship.png",
                        Type = typeof(ResourceTexture)
                    }
                });

                var dr = new DrawableSprite
                {
                    Scaling = new Vec2(0.5f, 0.5f),
                    Resource = entity.GetComponent<ResRef>()!.Test.GetResource<ResourceTexture>(),
                };
                
                
                var cont = new DrawableContainer()
                {
                    Pivot = new Vec2(131f / 2, 256f / 2)
                };

                List<IDrawable> guns = new();
                for (int i = 0; i < 10; i++)
                {
                    var dr2 = new DrawableSprite
                    {
                        Resource = entity.GetComponent<ResRef>()!.Test.GetResource<ResourceTexture>(),
                        Position = new Vec2(64f, 0f),
                        Scaling = new Vec2(0.2f, 0.2f)
                    };
                    cont.AddChild(dr2);
                    guns.Add(dr2);
                }

                cont.AddChild(dr);
                
                entity.AddComponent(new ClientTestComponent
                {
                    Drawable = cont,
                    Ch = guns
                });
            },
            (_, entity, _) =>
            {
                entity.RemoveComponent<ClientTestComponent>();
                entity.RemoveComponent<ResRef>();
            }
        );
    }

    public void Initialize(World world)
    {
        for (var i = 0; i < 1; i++)
        {
            var entity = world.CreateEntity();
            entity.AddComponent(new PositionComponent
            {
                X = 640/2,
                Y = 400,
                Index = i*10
            });
        }
    }

    private void Update(World world, MessageUpdate message)
    {
        foreach (var e in world.GetEntitiesWith<ClientTestComponent>().With<PositionComponent>())
        {
            var pc = e.GetComponent<PositionComponent>()!;
            foreach (var e2 in world.GetEntitiesWith<ComponentMainLoop>())
            {
                var speed = 4.0f;
                var dr = e.GetComponent<ClientTestComponent>()!;
                var ts = (DateTime.Now - e2.GetComponent<ComponentMainLoop>()!.StartTime).TotalSeconds;
                dr.Drawable.Position = new Vec2(pc.X, pc.Y);
                pc.X = 640f / 2f - (float)Math.Cos(ts*1)*260f;
                dr.Drawable.Rotation = (float) -Math.Cos(ts*4)/4;
                dr.Drawable.Scaling.Y = ((float) Math.Cos(ts*8))/6 + 1;
                int i = 0;
                foreach (var ch in dr.Ch)
                {
                    i++;
                    ch.Position.X = (float) Math.Cos(i*0.7 + ts * 2) * 20f + (float) Math.Cos(i*0.7 + ts * 2) * i*2;
                    ch.Position.Y = i * 7 - 60;
                    ch.ZOrder = (float) Math.Sin(i*0.7 + ts * 2) * 128f;
                }
            }
        }
    }

    private void Close(World world, MessageInputWindowClose messageInputWindowClose)
    {
        foreach (var p in world.GetEntitiesWith<PositionComponent>())
        {
            p.RemoveComponent<PositionComponent>();
        }
        
        world.GetSingletonComponent<ComponentMainLoop>()!.Running = false;
    }
}