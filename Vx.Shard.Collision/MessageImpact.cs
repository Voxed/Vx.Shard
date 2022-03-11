using Vx.Shard.Core;

namespace Vx.Shard.Collision;

public class MessageImpact : IMessage
{
    public Entity Entity1;
    public Entity Entity2;

    public MessageImpact(Entity e1, Entity e2)
    {
        this.Entity1 = e1;
        this.Entity2 = e2;
    }
}