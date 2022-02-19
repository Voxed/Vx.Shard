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
                    },
                    TestL = new ResourceReference
                    {
                        Path = "ship.png",
                        Type = typeof(ResourceTexture)
                    },
                    TestR = new ResourceReference
                    {
                        Path = "ship.png",
                        Type = typeof(ResourceTexture)
                    }
                });

                var dr = new DrawableSprite
                {
                    Scaling = new Vec2(0.4f, 0.4f),
                    Pivot = new Vec2(0.5f, 0.5f),
                    Resource = entity.GetComponent<ResRef>()!.Test.GetResource<ResourceTexture>(),
                };
                
                
                var cont = new DrawableContainer()
                {
                    Pivot = new Vec2(131f / 2, 256f / 2)
                };

                List<IDrawable> guns = new();
                for (int i = 0; i < 20; i++)
                {
                    var dr2 = new DrawableSprite
                    {
                        Resource = entity.GetComponent<ResRef>()!.Test.GetResource<ResourceTexture>(),
                        Position = new Vec2(64f, 0f),
                        Scaling = new Vec2(0.2f, 0.2f),
                        BlendMode = BlendMode.Add,
                        Pivot = new Vec2(0.5f, 0.5f)
                    };
                    cont.AddChild(dr2);
                    guns.Add(dr2);
                }

                cont.AddChild(dr);
                
                entity.AddComponent(new ClientTestComponent
                {
                    Drawable = cont,
                    Ch = guns,
                    Sh = dr,
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
                var ts = (DateTime.Now - e2.GetComponent<ComponentMainLoop>()!.StartTime).TotalSeconds*2;
                dr.Drawable.Position = new Vec2(pc.X, pc.Y - ((float) Math.Cos(ts*60 + Math.PI) + 1)*1.5f);
                pc.X = 640f / 2f - (float)Math.Cos(ts*1)*260f;
                dr.Drawable.Rotation = (float) -Math.Cos(ts*2);
                dr.Drawable.Scaling.Y = (1 - ((float) Math.Cos(ts*2 + Math.PI) + 1)/3.0f)*2.0f;
                dr.Drawable.Scaling.X = (1 - ((float) Math.Cos(ts*2 + Math.PI) + 1)/3.0f)*2.0f;
                dr.Drawable.Tint.G = (float)(Math.Cos(ts*2) + 1) / 2f;
                dr.Drawable.Tint.B = (float)(Math.Cos(ts*2) + 1) / 2f;
                if ((float) Math.Cos(ts * 1) > 0.3)
                {
                    dr.Sh.Resource = e.GetComponent<ResRef>()!.TestL.GetResource<ResourceTexture>();
                }
                else if((float) Math.Cos(ts * 1) < -0.3)
                {
                    dr.Sh.Resource = e.GetComponent<ResRef>()!.TestR.GetResource<ResourceTexture>();
                }
                else
                {
                    dr.Sh.Resource = e.GetComponent<ResRef>()!.Test.GetResource<ResourceTexture>();
                }
                int i = 0;
                foreach (var ch in dr.Ch)
                {
                    i++;
                    ch.Position.X = (float) Math.Cos(i*0.7 + ts * 2) * 40f + (float) Math.Cos(i*0.7 + ts * 2) * i*6;
                    ch.Position.Y = i * 4 - 60;
                    ch.ZOrder = (float) Math.Sin(i*0.7 + ts * 2) * 128f;
                    ch.Opacity = (float)(Math.Cos(ts*2) + 1) / 6f;
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