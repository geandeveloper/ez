using EzCommon.Events;
using EzPayment.Payments.CreatePix;

namespace EzPayment.Events.Payments
{
    public record PaymentCreatedEvent(string Id, CreatePaymentCommand Command) : Event;
}
