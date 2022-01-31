namespace Vx.Shard.Core;

public interface IMessageBusListener
{

    public void onMessage<T>(T message) where T : IMessage;
}

public class MessageBus
{
    private IMessageBusListener? listener;

    internal void SetListener(IMessageBusListener listener)
    {
        this.listener = listener;
    }

    internal void send<T>(T message) where T : IMessage
    {
        if(this.listener != null) {
            this.listener.onMessage<T>(message);
        }
    }
}