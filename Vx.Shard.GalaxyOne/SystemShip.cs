using Vx.Shard.Common;
using Vx.Shard.Core;
using Vx.Shard.Graphics;
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
    
    private void Update(World world, MessageUpdate _)
    {
        foreach (var entity in world.GetEntitiesWith<ComponentShipRenderer>())
        {
            entity.GetComponent<ComponentShipRenderer>()!.BoostSprite.Cell.X =
                (int) ((DateTime.Now - entity.GetComponent<ComponentShipRenderer>()!.CreationTime).TotalSeconds * 10)
                % entity.GetComponent<ComponentShipRenderer>()!.BoostAnimationLength;
            entity.GetComponent<ComponentRenderer>()!.DrawableContainer.Rotation =
                entity.GetComponent<Component2DVelocity>()!.Velocity.X/60.0f;
        }
    }
}