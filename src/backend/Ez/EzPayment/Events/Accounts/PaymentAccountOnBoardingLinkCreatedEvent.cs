using EzCommon.Events;

namespace EzPayment.Events.Accounts;

public record PaymentAccountOnBoardingLinkCreatedEvent(string PaymentAccountId, string OnBoardingLink) : Event;