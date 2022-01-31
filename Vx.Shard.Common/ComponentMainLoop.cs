namespace Vx.Shard.Common;

using Vx.Shard.Core;

public record ComponentMainLoop : IComponent
{
    public bool Running { get; set; } = true;
    public DateTime StartTime { get; init; }
}