using Vx.Shard.Common;
using Vx.Shard.Core;
using Vx.Shard.Graphics;
using Vx.Shard.Physics;

namespace Vx.Shard.GalaxyOne;

public class SystemEnemyShipControl : ISystem
{
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        componentRegistry.Register<ComponentEnemyShipController>();
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        messageBusListenerBuilder.AddCallback<MessageUpdate>(Update);
    }

    public void Initialize(World world)
    {
    }

    private void Update(World world, MessageUpdate msg)
    {
        foreach (var entity in world.GetEntitiesWith<ComponentEnemyShipController>().With<ComponentShipRenderer>())
        {
            // Renderer control here
            entity.GetComponent<ComponentShipRenderer>()!.Flipped = true;
            entity.GetComponent<ComponentShipRenderer>()!.Tint = new Color(1, 0.5f, 0.5f);
        }

        foreach (var entity in world.GetEntitiesWith<ComponentEnemyShipController>().With<ComponentPhysics>().With<
                     ComponentPosition>())
        {
            // Velocity control here
            var players = world.GetEntitiesWith<ComponentPlayerController>().With<ComponentPosition>();
            if (players.Any())
            {
                var target = players.MinBy(e =>
                {
                    var ppos = e.GetComponent<ComponentPosition>()!.Position;
                    var epos = entity.GetComponent<ComponentPosition>()!.Position;
                    var weight = (ppos - epos).Distance();
                    if (ppos.Y < epos.Y)
                        weight +=
                            1000000; // Add a lot of weight if this player is unreachable because its behind the ship.
                    return weight;
                });

                var ctrl = entity.GetComponent<ComponentEnemyShipController>()!;

                var newVelocity = ((new Vec2(target.GetComponent<ComponentPosition>()!.Position.X,
                                        entity.GetComponent<ComponentPosition>()!.Position.Y) -
                                    entity.GetComponent<ComponentPosition>()!.Position) / 8.0f) * 10f;

                if (target.GetComponent<ComponentPosition>()!.Position.Y <
                    entity.GetComponent<ComponentPosition>()!.Position.Y)
                {
                    newVelocity.X *= 0.1f;
                    newVelocity.Y = 1000f;
                }
                else
                {
                    newVelocity.Y = 700f;
                }
                
                if (newVelocity.Distance() > ctrl.MaxVelocity)
                {
                    newVelocity = newVelocity / newVelocity.Distance() * ctrl.MaxVelocity;
                }

                entity.GetComponent<ComponentPhysics>()!.Velocity +=
                    (newVelocity - entity.GetComponent<ComponentPhysics>()!.Velocity) *
                    (float) msg.Delta.TotalSeconds * 10;

                if (entity.GetComponent<ComponentPosition>()!.Position.Y > 800)
                {
                    entity.GetComponent<ComponentPosition>()!.Position.Y = -200 - (float) (new Random().NextDouble() * 500);
                    entity.GetComponent<ComponentPosition>()!.Position.X = (float) (new Random().NextDouble() * 1000);
                }
            }
        }
    }
}