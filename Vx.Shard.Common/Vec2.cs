// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

namespace Vx.Shard.Common;

/// <summary>
/// The Vec2 class models a 2D vector. Along with nice to have operators. 
/// </summary>
public class Vec2
{
    /// <summary>
    /// Construct a zero initialized 2D vector.
    /// </summary>
    public static Vec2 Zero => new(0, 0);

    /// <summary>
    /// The x component of the 2D vector.
    /// </summary>
    public float X;
    
    /// <summary>
    /// The y component of the 2D vector.
    /// </summary>
    public float Y;

    /// <summary>
    /// Construct a 2D vector.
    /// </summary>
    /// <param name="x">The x component.</param>
    /// <param name="y">The y component.</param>
    public Vec2(float x, float y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Add two vectors together.
    /// </summary>
    /// <param name="first">The first vector.</param>
    /// <param name="second">The second vector.</param>
    /// <returns>The new vector.</returns>
    public static Vec2 operator +(Vec2 first, Vec2 second)
    {
        return new Vec2(first.X + second.X, first.Y + second.Y);
    }
}