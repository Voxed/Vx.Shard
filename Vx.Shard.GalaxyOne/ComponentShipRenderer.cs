using Vx.Shard.Common;
using Vx.Shard.Core;
using Vx.Shard.Graphics;
using Vx.Shard.Resources;

namespace Vx.Shard.GalaxyOne;

public class ComponentShipRenderer : IComponent
{
    public readonly ResourceReference ShipTexture;
    public readonly DrawableSprite ShipSprite;

    public readonly ResourceReference BoostTexture;
    public readonly DrawableSprite BoostSprite;

    public readonly DateTime CreationTime = DateTime.Now;
    public readonly int BoostAnimationLength = 0;
    
    public ComponentShipRenderer(string shipTexturePath, string boostTexturePath, int boostAnimationLength)
    {
        BoostAnimationLength = boostAnimationLength;
        ShipTexture = new ResourceReference(shipTexturePath, "texture");
        ShipSprite = new DrawableSprite(ShipTexture)
        {
            Scaling = new Vec2(2),
            ScaleQuality = ScaleQuality.Nearest,
            Pivot = new Vec2(0.5f)
        };

        BoostTexture = new ResourceReference(boostTexturePath, "texture");
        BoostSprite = new DrawableSprite(BoostTexture)
        {
            Scaling = new Vec2(2),
            ScaleQuality = ScaleQuality.Nearest,
            Pivot = new Vec2(0.5f, 0.0f),
            Grid = new Vec2(BoostAnimationLength, 1),
            Position = new Vec2(0, 36)
        };
    }
}