using Vx.Shard.Common;
using Vx.Shard.Core;
using Vx.Shard.Graphics;
using Vx.Shard.Resources;

namespace Vx.Shard.SpaceDemo;

public class ComponentStar : IComponent, IDrawableComponent, IResourceComponent
{
    private ResourceReference _star = new ResourceReference("resources/textures/star.png", "texture");

    public readonly DrawableSprite Sprite;

    public IDrawable Drawable => Sprite;
    public ResourceReference[] Resources => new[] {_star};

    private float _redness;
    public float Redness
    {
        get => _redness;
        set
        {
            _redness = value;
            Sprite.Tint = new Color(1.0f, 1.0f - _redness, 1.0f - _redness);
        }
    }
    
    private float _size;
    public float Size
    {
        get => _size;
        set
        {
            _size = value;
            Sprite.Scaling = new Vec2(_size*0.25f, _size*Speed);
        }
    }

    public float Speed;

    private float _opacity;

    public float Opacity
    {
        get => _opacity;
        set
        {
            _opacity = value;
            Sprite.Opacity = _opacity;
        }
    }
    
    public ComponentStar(float size, float redness, float speed, float opacity)
    {
        Sprite = new DrawableSprite
        {
            Resource = _star,
            Pivot = new Vec2(0.5f, 0.5f),
        };
        Size = size;
        Redness = redness;
        Speed = speed;
        Opacity = opacity;
    }
}