using EzCommon.Events;

namespace EzGym.Payments.Events;

public record PixPaymentCreatedEvent(string PaymentId, Pix Pix) : Event;