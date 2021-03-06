// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Vx.Shard.Core.Specs")]
namespace Vx.Shard.Core;

/// <summary>
/// The IMessageBusListener describes the interface used to listen for bus messages.
/// </summary>
internal interface IMessageBusListener
{
    /// <summary>
    /// This method is invoked whenever a message is transmitted on the message bus.
    /// </summary>
    /// <param name="messageId">The id of the message received.</param>
    /// <param name="message">The message.</param>
    public void OnMessage(int messageId, IMessage message);
}

/// <summary>
/// The MessageBus handles messages transmitted to the world.
/// </summary>
internal class MessageBus
{
    private IMessageBusListener? _listener;

    /// <summary>
    /// Set the listener of the message bus.
    /// This replaces the current listener, so there can only be one listener.
    /// </summary>
    /// <param name="newListener">The new listener to use.</param>
    internal void SetListener(IMessageBusListener newListener)
    {
        _listener = newListener;
    }

    /// <summary>
    /// Send a message onto the message bus.
    /// </summary>
    /// <param name="messageId">The id of the message received.</param>
    /// <param name="message">The message.</param>
    internal void Send(int messageId, IMessage message)
    {
        _listener?.OnMessage(messageId, message);
    }
}