using System;
using EzCommon.Models;
using EzPayment.Events.Payments;
using EzPayment.Payments.CreatePix;

namespace EzPayment.Payments
{

    public class Payment : AggregateRoot
    {
        public string IntegrationId { get; private set; }
        public string Description { get; set; }
        public long Amount { get; protected set; }
        public PaymentStatusEnum PaymentStatus { get; private set; }
        public PaymentMethodEnum PaymentMethod { get; private set; }
        public Pix Pix { get; private set; }
        public CreditCard Card { get; private set; }
        public DateTime? PaymentDateTime { get; private set; }

        public Payment() { }

        public Payment(string integrationId, CreatePaymentCommand command)
        {
            RaiseEvent(new PaymentCreatedEvent(GenerateNewId(), integrationId, command));
        }

        public Payment PayWithPix(Pix pix)
        {
            RaiseEvent(new PixPaymentCreatedEvent(Id, pix));
            return this;
        }

        public Payment PayWithCreditCard(CreditCard card)
        {
            RaiseEvent(new CreditCardPaymentCreatedEvent(Id, card));
            return this;
        }

        public void Receive(long amount)
        {
            RaiseEvent(new PaymentReceivedEvent(Id, IntegrationId, amount, DateTime.UtcNow));
        }

        protected void Apply(PaymentReceivedEvent @event)
        {
            PaymentStatus = PaymentStatusEnum.Approved;
            PaymentDateTime = @event.PaymentDateTime;
        }

        protected void Apply(PaymentCreatedEvent @event)
        {
            Id = @event.Id;
            IntegrationId = @event.IntegrationId;
            Description = @event.Command.Description;
            PaymentStatus = PaymentStatusEnum.Pending;
            Amount = @event.Command.Amount;
            PaymentDateTime = null;
        }

        protected void Apply(PixPaymentCreatedEvent @event)
        {
            Pix = @event.Pix;
            PaymentMethod = PaymentMethodEnum.Pix;
        }

        protected void Apply(CreditCardPaymentCreatedEvent @event)
        {
            Card = @event.Card;
            PaymentMethod = PaymentMethodEnum.CreditCard;
        }
    }
}
