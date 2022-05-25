using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Spirebyte.Services.Repositories.Application.Services.Interfaces;

namespace Spirebyte.Services.Repositories.Tests.Shared;

public class TestMessageBroker : IMessageBroker
{
    private readonly List<IEvent> _events = new();

    public IReadOnlyList<IEvent> Events => _events;

    public Task PublishAsync(params IEvent[] events)
    {
        _events.AddRange(events);
        return Task.CompletedTask;
    }
}