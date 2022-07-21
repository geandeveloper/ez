using EzCommon.Commands;
using EzCommon.Events;
using EzCommon.Models;

namespace EzCommon.Infra.Bus;

public interface IBus
{
    Task PublishAsync(params IEvent[] events);
    Task<EventStream> RequestAsync(ICommand command);
}

