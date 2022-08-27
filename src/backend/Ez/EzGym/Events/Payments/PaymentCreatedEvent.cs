using EzCommon.Events;
using EzGym.Payments.CreatePix;

namespace EzGym.Events.Payments
{
    public record PaymentCreatedEvent(string Id, CreatePaymentCommand Command) : Event;
}
