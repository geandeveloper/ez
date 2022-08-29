using System;
using EzCommon.Events;

namespace EzPayment.Events.Payments;

public record PaymentReceivedEvent(string PaymentId, long Amount, DateTime PaymentDateTime) : Event;