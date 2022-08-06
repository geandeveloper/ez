using EzCommon.Events;
using System.Threading.Tasks;

namespace EzCommon.Infra.Bus;

public interface IBus
{
    Task PublishAsync<TEvent>(params TEvent[] events) where TEvent : IEvent;
}

