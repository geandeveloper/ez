using EzCommon.Events;
using EzPayment.Payments.CreatePayment;

namespace EzPayment.Events.Payments
{
    public record PaymentCreatedEvent(string Id, long ApplicationFeeAmount, CreatePaymentCommand Command) : Event;
}
