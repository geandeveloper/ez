using EzCommon.Events;
using System;
using EzGym.Payments.CreatePayment;

namespace EzGym.Payments.Events
{
    public record PaymentCreatedEvent(Guid Id, CreatePaymentCommand Command, PaymentReceipt Receipt) : Event;
}
