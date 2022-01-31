// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

namespace Vx.Shard.Graphics;

public interface IDrawableVisitor<in T>
{
    public void VisitSprite(T context, DrawableSprite sprite);
    public void VisitContainer(T context, DrawableContainer container);
}