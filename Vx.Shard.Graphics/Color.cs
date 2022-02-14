namespace Vx.Shard.Graphics;

public class Color
{
    public static Color White => new(
        1.0f,
        1.0f,
        1.0f
    );

    public Color(float r, float g, float b)
    {
        R = r;
        G = g;
        B = b;
    }

    public float R;
    public float G;
    public float B;

    public static Color operator *(Color first, Color second)
    {
        return new Color(first.R * second.R, first.G * second.G, first.B * second.B);
    }
}