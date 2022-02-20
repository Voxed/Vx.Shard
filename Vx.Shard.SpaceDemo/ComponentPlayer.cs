using Vx.Shard.Common;
using Vx.Shard.Core;
using Vx.Shard.Graphics;
using Vx.Shard.Resources;

namespace Vx.Shard.SpaceDemo;

public record ComponentPlayer : IComponent, IDrawableComponent, IResourceComponent
{
    public ResourceReference ForwardTexture = new ResourceReference
    (
        "resources/textures/player/0001.png",
        "texture"
    );

    public ResourceReference RightTexture = new ResourceReference
    (
        "resources/textures/player/0002.png",
        "texture"
    );

    public ResourceReference LeftTexture = new ResourceReference
    (
        "resources/textures/player/0003.png",
        "texture"
    );

    public ResourceReference BackRightTexture = new ResourceReference
    (
        "resources/textures/player/0005.png",
        "texture"
    );

    public ResourceReference BackLeftTexture = new ResourceReference
    (
        "resources/textures/player/0006.png",
        "texture"
    );

    public ResourceReference BackForwardTexture = new ResourceReference
    (
        "resources/textures/player/0004.png",
        "texture"
    );

    public List<ResourceReference> SpinningTextures = new();
    private List<ResourceReference> _resources = new();

    public readonly DrawableSprite Sprite;

    public float SpinningTimer = 0;
    public bool Spinning = false;

    public Vec2 Speed = Vec2.Zero;

    public ComponentPlayer()
    {
        Sprite = new DrawableSprite
        {
            Resource = ForwardTexture,
            Scaling = new Vec2(0.35f, 0.35f),
            Pivot = new Vec2(0.5f, 0.5f),
            ZOrder = 10
        };

        for (int i = 7; i < 21; i++)
        {
            SpinningTextures.Add(new ResourceReference($"resources/textures/player/{i}.png", "texture"));
            _resources.Add(SpinningTextures.Last());
        }

        _resources.AddRange(new[]
            {ForwardTexture, RightTexture, LeftTexture, BackRightTexture, BackLeftTexture, BackForwardTexture});
    }

    public IDrawable Drawable => Sprite;

    public ResourceReference[] Resources => _resources.ToArray();
};