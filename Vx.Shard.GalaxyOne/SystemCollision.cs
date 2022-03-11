using Vx.Shard.Common;
using Vx.Shard.Core;

namespace Vx.Shard.GalaxyOne;

public class SystemCollision : ISystem
{
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        componentRegistry.Register<ComponentCollision>();
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
        var colliders = world
            .GetEntitiesWith<ComponentCollision>()
            .With<ComponentVelocity>()
            .With<ComponentPosition>().ToList();

        for(var i = 0; i < colliders.Count(); i++)
        {
            for (var j = i + 1; j < colliders.Count(); j++)
            {
                var e1 = colliders[i];
                var e2 = colliders[j];

                var c1 = e1.GetComponent<ComponentCollision>()!;
                var c2 = e2.GetComponent<ComponentCollision>()!;

                var p1 = e1.GetComponent<ComponentPosition>()!;
                var p2 = e2.GetComponent<ComponentPosition>()!;

                var cv1 = e1.GetComponent<ComponentVelocity>()!;
                var cv2 = e2.GetComponent<ComponentVelocity>()!;
                
                var m1 = Math.Pow(c1.Radius, 2) * Math.PI;
                var m2 = Math.Pow(c2.Radius, 2) * Math.PI;

                var a1 = cv1.Velocity.Angle();
                var a2 = cv2.Velocity.Angle();

                var v1 = cv1.Velocity.Distance();
                var v2 = cv2.Velocity.Distance();

                var g = 0;
                
                if ((p1.Position - p2.Position).Distance() < c1.Radius + c2.Radius)
                {
                    // They are colliding.
                    /*cv1.Velocity = new Vec2(
                        (float) ((v1*Math.Cos(a1 - g) * (m1 - m2) + 2*m2*v2*Math.Cos(a2-g))/(m1 + m2)*Math.Cos(g) + v1*Math.Sin(a1-g)*Math.Cos(g+Math.PI/2)),
                        (float) ((v1*Math.Cos(a1 - g) * (m1 - m2) + 2*m2*v2*Math.Cos(a2-g))/(m1 + m2)*Math.Sin(g) + v1*Math.Sin(a1-g)*Math.Sin(g+Math.PI/2))
                    );*/
                    
                    cv2.Velocity = new Vec2(
                        (float) ((v2*Math.Cos(a2 - g) * (m2 - m1) + 2*m1*v1*Math.Cos(a1-g))/(m1 + m2)*Math.Cos(g) + v2*Math.Sin(a2-g)*Math.Cos(g+Math.PI/2)),
                        (float) ((v2*Math.Cos(a2 - g) * (m2 - m1) + 2*m1*v1*Math.Cos(a1-g))/(m1 + m2)*Math.Sin(g) + v2*Math.Sin(a2-g)*Math.Sin(g+Math.PI/2))
                    );
                    
                    Console.WriteLine("==========");
                    Console.WriteLine(cv2.Velocity);
                    Console.WriteLine(cv1.Velocity);
                }
            }
        }
    }
}