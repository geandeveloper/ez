using EzCommon.Events;
using EzGym.Payments;

namespace EzGym.Events.Payments;

public record PixPaymentCreatedEvent(string PaymentId, Pix Pix) : Event;