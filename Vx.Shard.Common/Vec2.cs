namespace Vx.Shard.Common;

public class Vec2
{

    public static Vec2 ZERO { get { return new Vec2(0, 0); } }

    public float X, Y;

    public Vec2(float x, float y)
    {
        this.X = x;
        this.Y = y;
    }

    public static Vec2 operator +(Vec2 first, Vec2 second)
    {
        return new Vec2(first.X + second.X, first.Y + second.Y);
    }

}