namespace Vx.Shard.Core;

public class MessageRegistry
{
    private readonly Dictionary<Type, int> _messages = new ();
    private int _nextMessageId = 0;
    
    internal MessageRegistry()
    {
    }

    public void Register<T>() where T : IMessage
    {
        if (!_messages.ContainsKey(typeof(T)))
            _messages.Add(typeof(T), ++_nextMessageId); // No zero id, that will indicate a null component.
        
        Console.WriteLine("    * Registered message {0} to id {1}", typeof(T).Name, _nextMessageId);
    }
    
    internal int GetMessageId<T>() where T : IMessage
    {
        return _messages.ContainsKey(typeof(T)) ? _messages[typeof(T)] : 0;
    }

    internal int[] GetSubclassMessageIds<T>()
    {
        return (from pair in _messages where pair.Key.IsSubclassOf(typeof(T)) select pair.Value).ToArray();
    }
}