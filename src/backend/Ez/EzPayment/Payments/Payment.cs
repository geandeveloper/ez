using System;
using EzCommon.Models;
using EzPayment.Events.Payments;
using EzPayment.Payments.CreatePix;

namespace EzPayment.Payments
{

    public class Payment : AggregateRoot
    {
        public string Description { get; set; }
        public decimal Value { get; protected set; }
        public PaymentStatusEnum PaymentStatus { get; private set; }
        public PaymentMethodEnum PaymentMethod { get; private set; }
        public Pix Pix { get; private set; }
        public DateTime? PaymentDateTime { get; private set; } 

        public Payment() { }

        public Payment(CreatePaymentCommand command)
        {
            RaiseEvent(new PaymentCreatedEvent(GenerateNewId(), command));
        }

        public Payment CreatePix(Pix pix)
        {
            RaiseEvent(new PixPaymentCreatedEvent(Id, pix));
            return this;
        }

        protected void Apply(PaymentCreatedEvent @event)
        {
            Id = @event.Id;
            Description = @event.Command.Description;
            PaymentStatus = PaymentStatusEnum.Pending;
            Value = @event.Command.Value;
            PaymentDateTime = null;
        }

        protected void Apply(PixPaymentCreatedEvent @event)
        {
            Pix = @event.Pix;
            PaymentMethod = PaymentMethodEnum.Pix;
        }
    }
}
