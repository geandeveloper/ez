using EzCommon.Events;
using MediatR;
using System.Linq;
using System.Threading.Tasks;

namespace EzCommon.Infra.Bus;

public class InMemoryBus : IBus
{
    private readonly IMediator _mediator;

    public InMemoryBus(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task PublishAsync<TEvent>(params TEvent[] events) where TEvent : IEvent
    {
        return Task.WhenAll(events.Select(e => _mediator.Publish(e)));
    }
}


