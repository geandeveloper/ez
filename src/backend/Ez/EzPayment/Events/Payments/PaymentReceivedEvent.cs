using System;
using EzCommon.Events;

namespace EzPayment.Events.Payments;

public record PaymentReceivedEvent(string PaymentId, string IntegrationId, long Amount, DateTime PaymentDateTime) : Event;