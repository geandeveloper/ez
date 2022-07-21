using EzCommon.Commands;
using EzCommon.Events;
using EzCommon.Infra.Bus;
using EzCommon.Models;
using MediatR;
using System.Linq;
using System.Threading.Tasks;

namespace EzIdentity.Infra.Bus;

public class InMemoryBus : IBus
{
    private readonly IMediator _mediator;

    public InMemoryBus(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task PublishAsync(params IEvent[] events)
    {
        await Task.WhenAll(events.Select(e => _mediator.Publish(e)));
    }

    public async Task<EventStream> RequestAsync(ICommand command)
    {
        return await _mediator.Send(command);
    }
}

