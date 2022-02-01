namespace Vx.Shard.Core.Specs;

using System.Linq;
using Xunit;

public class WorldShould
{
    private class TestMessage : IMessage
    {
    }

    private class TestComponent : IComponent
    {
    }

    private readonly World _world;

    public WorldShould()
    {
        _world = new World();
    }

    [Fact]
    public void SendToMessageBus()
    {
        var messageReceived = false;
        var builder = new MessageBusListenerBuilder();
        builder.AddCallback<TestMessage>((_, _) => { messageReceived = true; });
        _world.MessageBus.SetListener(builder.Build(_world));
        _world.Send(new TestMessage());
        Assert.True(messageReceived);
    }

    [Fact]
    public void CreateUniqueEntities()
    {
        Assert.Equal(
            10,
            Enumerable.Range(0, 10).Select(_ => _world.CreateEntity().Id).Distinct().Count()
        );
    }

    [Fact]
    public void FindEntitiesWithComponent()
    {
        var entitiesWithComponent = Enumerable.Range(0, 10).Select(_ => _world.CreateEntity()).ToList();
        entitiesWithComponent.ForEach(e => e.AddComponent(new TestComponent()));
        Enumerable.Range(0, 10).ToList().ForEach(_ => _world.CreateEntity());
        Assert.Equal(
            entitiesWithComponent.ToList().Select(e => e.Id).OrderBy(e => e).ToList(),
            _world.GetEntitiesWith<TestComponent>().ToList().Select(e => e.Id).OrderBy(e => e).ToList()
        );
    }
}