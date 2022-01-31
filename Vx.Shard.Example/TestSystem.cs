namespace Vx.Shard.Example;

using Vx.Shard.Core;
using Vx.Shard.Common;

public class TestSystem : ISystem
{

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder, ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        Console.WriteLine("Configured!");

        messageBusListenerBuilder.AddCallback<UpdateMessage>(this.onUpdateMessage);
        messageBusListenerBuilder.AddCallback<MessageUpdate>(this.update);
        componentStoreListenerBuilder.AddCallback<PositionComponent>(((World world, Entity entity) =>
        {
            entity.AddComponent(new ClientTestComponent
            {
                wow = "Hello!"
            });
        }, (World world, Entity entity) =>
        {
            entity.RemoveComponent<ClientTestComponent>();
        }
        ));
    }

    public void Initialize(World world)
    {
        for (int i = 0; i < 100; i++)
        {
            Entity entity = world.CreateEntity();
            if (i >= 5)
            {
                entity.AddComponent(new PositionComponent
                {
                    x = 12,
                    y = 34
                });
                entity.GetComponent<PositionComponent>()!.x = 43;
            }
        }
    }

    public void update(World world, MessageUpdate message)
    {
        int i = 0;
        world.GetEntitiesWith<ClientTestComponent>().ToList().ForEach(e =>
        {
            i++;
            world.GetEntitiesWith<ComponentMainLoop>().ToList().ForEach(e2 =>
            {
                e.GetComponent<ClientTestComponent>()!.drawable.Position.X =
                    (int)(Math.Cos((DateTime.Now - e2.GetComponent<ComponentMainLoop>()!.StartTime).TotalSeconds + i*0.05) * 200.0) + 280;
                e.GetComponent<ClientTestComponent>()!.drawable.Position.Y =
                    (int)(Math.Sin((DateTime.Now - e2.GetComponent<ComponentMainLoop>()!.StartTime).TotalSeconds + i*0.05) * 200.0) + 200;
            });
        });
    }

    public void onUpdateMessage(World world, UpdateMessage message)
    {
        Console.WriteLine(message);

        world.GetEntitiesWith<PositionComponent>().ToList().ForEach(entity =>
        {
            Console.WriteLine(entity.GetComponent<PositionComponent>());
        });
        world.GetEntitiesWith<ClientTestComponent>().ToList().ForEach(entity =>
        {
            Console.WriteLine(entity.GetComponent<ClientTestComponent>());
        });
        world.GetEntitiesWith<PositionComponent>().ToList().ForEach(entity =>
        {
            entity.RemoveComponent<PositionComponent>();
        });
        Console.WriteLine("After removal");
        world.GetEntitiesWith<PositionComponent>().ToList().ForEach(entity =>
        {
            Console.WriteLine(entity.GetComponent<PositionComponent>());
        });
        world.GetEntitiesWith<ClientTestComponent>().ToList().ForEach(entity =>
        {
            Console.WriteLine(entity.GetComponent<ClientTestComponent>());
        });
    }
}