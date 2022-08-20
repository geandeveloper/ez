using EzCommon.Models;
using EzGym.Features.Payments.Pix;
using System;

namespace EzGym.Features.Payments
{
    public class Payment : AggregateRoot
    {
        public Payer Payer { get; private set; }
        public Receiver Receiver { get; private set; }
        public PaymentMethodEnum PaymentMethod { get; private set; }
        public decimal Value { get; private set; }

        public Payment() { }

        public Payment(CreatePixCommand command)
        {
            RaiseEvent(new PixCreatedEvent(Guid.NewGuid(), command));
        }

        protected void Apply(PixCreatedEvent @event)
        {
            Id = @event.Id;
            Payer = new Payer(@event.Command.PayerAccountId);
            Receiver = new Receiver(@event.Command.ReceiverAccountId);
            PaymentMethod = PaymentMethodEnum.Pix;
            Value = @event.Command.Value;
        }
    }
}
