using EzCommon.Models;
using EzGym.Payments.CreatePix;
using EzGym.Payments.Events;

namespace EzGym.Payments
{

    public class Payment : AggregateRoot
    {
        public string Description { get; set; }
        public decimal Value { get; protected set; }
        public PaymentStatusEnum PaymentStatus { get; private set; }
        public Pix Pix { get; private set; }

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
        }

        protected void Apply(PixPaymentCreatedEvent @event)
        {
            Pix = @event.Pix;
        }
    }
}
