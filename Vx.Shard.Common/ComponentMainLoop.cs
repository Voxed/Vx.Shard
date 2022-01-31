// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

namespace Vx.Shard.Common;

using Core;

/// <summary>
/// Component for controlling the main game loop.
/// </summary>
public record ComponentMainLoop : IComponent
{
    /// <summary>
    /// Whether the main loop should be running.
    /// </summary>
    public bool Running { get; set; } = true;

    /// <summary>
    /// The time when the main loop was started.
    /// </summary>
    public DateTime StartTime { get; init; }
}