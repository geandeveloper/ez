using EzCommon.Models;
using System;
using EzGym.Payments.CreatePayment;
using EzGym.Payments.Events;

namespace EzGym.Payments
{
    public class Payment : AggregateRoot
    {
        public Payer Payer { get; private set; }
        public Receiver Receiver { get; private set; }
        public PaymentMethodEnum PaymentMethod { get; private set; }
        public decimal Value { get; private set; }
        public PaymentReceipt Receipt { get; private set; }

        public Payment() { }

        public Payment(CreatePaymentCommand command, PaymentReceipt receipt)
        {
            RaiseEvent(new PaymentCreatedEvent(Guid.NewGuid(), command, receipt));
        }

        protected void Apply(PaymentCreatedEvent @event)
        {
            Id = @event.Id;
            Payer = new Payer(@event.Command.PayerAccountId);
            Receiver = new Receiver(@event.Command.ReceiverAccountId);
            PaymentMethod = PaymentMethodEnum.Pix;
            Value = @event.Command.Value;
            Receipt = @event.Receipt;
        }
    }
}
