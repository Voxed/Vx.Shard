namespace Vx.Shard.Core;

public class MessageBusListenerBuilder
{
    private class ConcreteMessageBusListener : IMessageBusListener {

        private readonly World world;
        private readonly Dictionary<Type, List<object>> callbacks;

        public ConcreteMessageBusListener(World world, Dictionary<Type, List<object>> callbacks) {
            this.world = world;
            this.callbacks = callbacks;
        }

        public void onMessage<T>(T message) where T : IMessage
        {
            if(callbacks.ContainsKey(typeof(T))) {
                callbacks[typeof(T)].ForEach(cb => {
                    ((Callback<T>)cb)(world, message);
                });
            }
        }
    }

    public delegate void Callback<T>(World world, T message);

    private readonly Dictionary<Type, List<object>> callbacks =
    new Dictionary<Type, List<object>>();

    public MessageBusListenerBuilder AddCallback<T>(Callback<T> callback) where T : IMessage
    {
        if (!callbacks.ContainsKey(typeof(T)))
        {
            callbacks.Add(typeof(T), new List<object>());
        }

        callbacks[typeof(T)].Add(callback);

        return this;
    }

    public IMessageBusListener Build(World world)
    {
        return new ConcreteMessageBusListener(world, callbacks);
    }
}