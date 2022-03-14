using Vx.Shard.Common;
using Vx.Shard.Core;
using Vx.Shard.Graphics;
using Vx.Shard.Resources;

namespace Vx.Shard.GalaxyOne;

public class SystemGuns : ISystem
{
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        componentRegistry.Register<ComponentGuns>();
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        messageBusListenerBuilder.AddCallback<MessageUpdate>(Update);

        componentStoreListenerBuilder.AddCallback<ComponentGuns>((world, entity, component) =>
            {
                foreach (var componentGunTexture in component.GunTextures)
                {
                    entity.GetComponent<ComponentResources>()!.Resources.Add(componentGunTexture);
                }


                for (var i = 0; i < 5; i++)
                {
                    var s = new DrawableSprite(component.GunTextures.First())
                    {
                        Scaling = new Vec2(2.0f),
                        ScaleQuality = ScaleQuality.Nearest,
                        Pivot = new Vec2(0.5f)
                    };
                    component.GunSprites.Add(s);
                    entity.GetComponent<ComponentRenderer>()!.DrawableContainer.AddChild(s);
                }
            },
            (world, entity, component) =>
            {
                foreach (var componentGunTexture in component.GunTextures)
                {
                    entity.GetComponent<ComponentResources>()!.Resources.Remove(componentGunTexture);
                }

                foreach (var s in component.GunSprites)
                {
                    entity.GetComponent<ComponentRenderer>()!.DrawableContainer.RemoveChild(s);
                }
            });
    }

    public void Initialize(World world)
    {
    }

    private void Update(World world, MessageUpdate msg)
    {
        foreach (var e in world.GetEntitiesWith<ComponentGuns>())
        {
            var guns = e.GetComponent<ComponentGuns>();

            for (var i = 0; i < guns.GunSprites.Count(); i++)
            {
                var x = (DateTime.Now - world.GetSingletonComponent<ComponentMainLoop>()!.StartTime).TotalSeconds +
                        i * 2 * 0.5f;
                guns.GunSprites[i].Position.X =
                    (float) Math.Cos(x) *
                    (48f + i * 2);
                guns.GunSprites[i].Position.Y = i * 2 + 32;
                guns.GunSprites[i].ZOrder =
                    (int) (Math.Sin(x) * (1000 + i * 2));
                guns.GunSprites[i].Tint = new Color((float) (Math.Sin(x) + 1f) / 4 + 0.5f,
                    (float) (Math.Sin(x) + 1f) / 4 + 0.5f,
                    (float) (Math.Sin(x) + 1f) / 4 + 0.5f);
            }
        }
    }
}