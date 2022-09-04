using System;
using EzCommon.Models;
using EzPayment.Events.Payments;
using EzPayment.Payments.CreatePayment;

namespace EzPayment.Payments
{

    public class Payment : AggregateRoot
    {
        public string Description { get; set; }
        public long Amount { get; private set; }
        public long ApplicationFeeAmount { get; private set; }
        public PaymentStatusEnum Status { get; private set; }
        public PaymentMethodEnum Method { get; private set; }
        public PixInfo PixInfo { get; private set; }
        public CreditCardInfo CardInfo { get; private set; }
        public DateTime? PaymentDateTime { get; private set; }
        public string RedirectUrl { get; set; }

        public Payment() { }

        public Payment(CreatePaymentCommand command, long applicationFeeAmount)
        {
            RaiseEvent(new PaymentCreatedEvent(GenerateNewId(), applicationFeeAmount, command));
        }

        public Payment PayWithPix(PixInfo pixInfo)
        {
            RaiseEvent(new PixPaymentCreatedEvent(Id, pixInfo));
            return this;
        }

        public Payment PayWithCreditCard(CreditCardInfo cardInfo)
        {
            RaiseEvent(new CreditCardPaymentCreatedEvent(Id, cardInfo));
            return this;
        }

        public void Receive(long amount)
        {
            RaiseEvent(new PaymentReceivedEvent(Id, amount, DateTime.UtcNow));
        }

        protected void Apply(PaymentReceivedEvent @event)
        {
            Status = PaymentStatusEnum.Approved;
            PaymentDateTime = @event.PaymentDateTime;
        }

        protected void Apply(PaymentCreatedEvent @event)
        {
            Id = @event.Id;
            Description = @event.Command.Description;
            Status = PaymentStatusEnum.Pending;
            Amount = @event.Command.Amount;
            ApplicationFeeAmount = @event.ApplicationFeeAmount;
            PaymentDateTime = null;
            RedirectUrl = @event.Command.RedirectUrl;
        }

        protected void Apply(PixPaymentCreatedEvent @event)
        {
            PixInfo = @event.PixInfo;
            Method = PaymentMethodEnum.Pix;
        }

        protected void Apply(CreditCardPaymentCreatedEvent @event)
        {
            CardInfo = @event.CardInfo;
            Method = PaymentMethodEnum.CreditCard;
        }
    }
}
