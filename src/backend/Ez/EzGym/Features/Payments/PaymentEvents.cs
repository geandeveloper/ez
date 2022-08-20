using EzCommon.Events;
using EzGym.Features.Payments.Pix;
using System;

namespace EzGym.Features.Payments
{
    public record PixCreatedEvent(Guid Id, CreatePixCommand Command) : Event;
}
