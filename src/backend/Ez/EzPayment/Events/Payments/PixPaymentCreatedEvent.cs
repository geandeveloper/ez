using EzCommon.Events;
using EzPayment.Payments;

namespace EzPayment.Events.Payments;

public record PixPaymentCreatedEvent(string PaymentId, Pix Pix) : Event;