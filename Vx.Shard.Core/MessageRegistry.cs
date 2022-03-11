namespace Vx.Shard.Core;

public class MessageRegistry
{
    private readonly Dictionary<Type, int> _messages = new();
    private int _nextMessageId;

    /// <summary>
    /// Register a message type to a message id.
    /// </summary>
    /// <typeparam name="T">The type to register.</typeparam>
    public MessageRegistry Register<T>() where T : IMessage
    {
        if (!_messages.ContainsKey(typeof(T)))
        {
            _messages.Add(typeof(T), ++_nextMessageId); // No zero id, that will indicate a null component.

            Console.WriteLine("    * Registered message {0} to id {1}", typeof(T).Name, _nextMessageId);
        }
        else
        {
            Console.WriteLine("    ! Message {0} has already been registered", typeof(T).Name);
        }

        return this;
    }

    /// <summary>
    /// Get the message id from a message type.
    /// </summary>
    /// <typeparam name="T">The message type.</typeparam>
    /// <returns>The message id.</returns>
    internal int GetMessageId<T>() where T : IMessage
    {
        return _messages.ContainsKey(typeof(T)) ? _messages[typeof(T)] : 0;
    }

    /// <summary>
    /// Get the id of all messages that are assignable to T.
    /// </summary>
    /// <typeparam name="T">The type to be assignable to.</typeparam>
    /// <returns>The message ids.</returns>
    internal int[] GetSubclassMessageIds<T>()
    {
        return (from pair in _messages where typeof(T).IsAssignableFrom(pair.Key) select pair.Value).ToArray();
    }
}