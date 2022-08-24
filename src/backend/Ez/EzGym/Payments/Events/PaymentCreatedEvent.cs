using EzCommon.Events;
using EzGym.Payments.CreatePix;

namespace EzGym.Payments.Events
{
    public record PaymentCreatedEvent(string Id, CreatePaymentCommand Command) : Event;
}
