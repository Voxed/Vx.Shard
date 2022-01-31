namespace Vx.Shard.Example;

using Core;
using Common;

public class TestSystem : ISystem
{
    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        messageBusListenerBuilder.AddCallback<MessageUpdate>(Update);
        componentStoreListenerBuilder.AddCallback<PositionComponent>((
            (_, entity) =>
            {
                entity.AddComponent(new ClientTestComponent());
            }, (_, entity) => { entity.RemoveComponent<ClientTestComponent>(); }
        ));
    }

    public void Initialize(World world)
    {
        for (var i = 0; i < 200; i++)
        {
            var entity = world.CreateEntity();
            if (i < 5) continue;
            entity.AddComponent(new PositionComponent
            {
                X = 12,
                Y = 34
            });
            entity.GetComponent<PositionComponent>()!.X = 43;
        }
    }

    private void Update(World world, MessageUpdate message)
    {
        var i = 0;
        world.GetEntitiesWith<ClientTestComponent>().ToList().ForEach(e =>
        {
            i++;
            world.GetEntitiesWith<ComponentMainLoop>().ToList().ForEach(e2 =>
            {
                e.GetComponent<ClientTestComponent>()!.Drawable.Position.X =
                    (int) (Math.Cos((DateTime.Now - e2.GetComponent<ComponentMainLoop>()!.StartTime).TotalSeconds +
                                    i * 0.1) * (350.0 - i * 1.5)) + 290;
                e.GetComponent<ClientTestComponent>()!.Drawable.Position.Y =
                    (int) (Math.Sin((DateTime.Now - e2.GetComponent<ComponentMainLoop>()!.StartTime).TotalSeconds +
                                    i * 0.1) * (350.0 - i * 1.5)) + 180;
            });
        });
    }
}