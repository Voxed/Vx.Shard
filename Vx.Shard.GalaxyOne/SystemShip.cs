using Vx.Shard.Collision;
using Vx.Shard.Common;
using Vx.Shard.Core;
using Vx.Shard.Graphics;
using Vx.Shard.Physics;
using Vx.Shard.Resources;

namespace Vx.Shard.GalaxyOne;

public class SystemShip : ISystem
{
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        componentRegistry.Register<ComponentShipRenderer>();
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        messageBusListenerBuilder.AddCallback<MessageUpdate>(Update);
        messageBusListenerBuilder.AddCallback<MessageImpact>(Impact);
        componentStoreListenerBuilder.AddCallback<ComponentShipRenderer>(
            (world, entity, component) =>
            {
                entity.GetComponent<ComponentResources>()?.Resources.Add(component.ShipTexture);
                entity.GetComponent<ComponentRenderer>()?.DrawableContainer.AddChild(component.ShipSprite);
                entity.GetComponent<ComponentResources>()?.Resources.Add(component.BoostTexture);
                entity.GetComponent<ComponentRenderer>()?.DrawableContainer.AddChild(component.BoostSprite);
            },
            (world, entity, component) =>
            {
                entity.GetComponent<ComponentResources>()?.Resources.Remove(component.ShipTexture);
                entity.GetComponent<ComponentRenderer>()?.DrawableContainer.RemoveChild(component.ShipSprite);
                entity.GetComponent<ComponentResources>()?.Resources.Remove(component.BoostTexture);
                entity.GetComponent<ComponentRenderer>()?.DrawableContainer.RemoveChild(component.BoostSprite);
            }
        );
    }

    public void Initialize(World world)
    {
    }

    private void Update(World world, MessageUpdate msg)
    {
        foreach (var entity in world.GetEntitiesWith<ComponentShipRenderer>())
        {
            entity.GetComponent<ComponentShipRenderer>()!.BoostSprite.Cell.X =
                (int) ((DateTime.Now - entity.GetComponent<ComponentShipRenderer>()!.CreationTime).TotalSeconds * 10)
                % entity.GetComponent<ComponentShipRenderer>()!.BoostAnimationLength;
            entity.GetComponent<ComponentRenderer>()!.DrawableContainer.Rotation =
                (float) (entity.GetComponent<ComponentPhysics>()!.Velocity.X / 2000.0f *
                         (entity.GetComponent<ComponentShipRenderer>()!.Flipped ? -1 : 1) +
                         (entity.GetComponent<ComponentShipRenderer>()!.Flipped ? Math.PI : 0) +
                         entity.GetComponent<ComponentShipRenderer>()!.Rotation);

            var dmgState = entity.GetComponent<ComponentShipRenderer>()!.DamageState;
            entity.GetComponent<ComponentRenderer>()!.DrawableContainer.Scaling = new Vec2(1 + dmgState * 0.1f);
            entity.GetComponent<ComponentShipRenderer>()!.ShipSprite.Tint =
                new Color(1.0f, 1 - dmgState * 0.5f, 1 - dmgState * 0.5f);

            entity.GetComponent<ComponentShipRenderer>()!.DamageState *= (float) Math.Pow(0.05, msg.Delta.TotalSeconds);
        }
    }

    private void Impact(World world, MessageImpact msg)
    {
        if (msg.Entity1.GetComponent<ComponentShipRenderer>() != null)
        {
            msg.Entity1.GetComponent<ComponentShipRenderer>()!.DamageState = 1;
        }
    }
}