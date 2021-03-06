// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Vx.Shard.Core.Specs")]
namespace Vx.Shard.Core;

/// <summary>
/// The base interface of all engine systems.
/// </summary>
public interface ISystem
{
    /// <summary>
    /// Register messages and components connected to the system.
    /// </summary>
    /// <param name="messageRegistry">The message registry.</param>
    /// <param name="componentRegistry">The component registry.</param>
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry);

    /// <summary>
    /// Configure the message and component listeners.
    /// </summary>
    /// <param name="messageBusListenerBuilder">The message bus listener builder.</param>
    /// <param name="componentStoreListenerBuilder">The component store listener builder.</param>
    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder);

    /// <summary>
    /// Do world initializations.
    /// </summary>
    /// <param name="world">The world where the system resides.</param>
    public void Initialize(World world);
}