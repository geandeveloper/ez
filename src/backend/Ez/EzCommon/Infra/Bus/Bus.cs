using EzCommon.Commands;
using EzCommon.Events;
using EzCommon.Models;
using MediatR;

namespace EzCommon.Infra.Bus;

public class Bus : IBus
{
    private readonly ISender _mediator;

    public Bus(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task PublishAsync(params IEvent[] events)
    {
        await Task.WhenAll(events.Select(e => _mediator.Send(e)));
    }

    public async Task<EventStream> RequestAsync(ICommand command)
    {
        return await _mediator.Send(command);
    }
}

