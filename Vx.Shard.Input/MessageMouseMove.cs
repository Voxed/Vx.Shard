using Vx.Shard.Common;
using Vx.Shard.Core;

namespace Vx.Shard.Window;

public class MessageMouseMove : IMessage
{
    public Vec2 Position;
    public MessageMouseMove(Vec2 position)
    {
        this.Position = position;
    }
}