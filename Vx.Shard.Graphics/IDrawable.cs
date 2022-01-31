namespace Vx.Shard.Graphics;

public interface IDrawable {
    void Accept<T>(T context, IDrawableVisitor<T> visitor);    
}