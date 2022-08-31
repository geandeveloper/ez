using EzCommon.Models;
using EzPayment.Events.Accounts;

namespace EzPayment.PaymentAccounts
{
    public class PaymentAccount : AggregateRoot
    {
        public PaymentAccountIntegrationInfo IntegrationInfo { get; private set; }
        public PaymentAccountStatusEnum Status { get; set; }

        public PaymentAccount() { }
        public PaymentAccount(string integrationId)
        {
            RaiseEvent(new PaymentAccountCreatedEvent(GenerateNewId(), integrationId));
        }

        public void UpdateStatus(PaymentAccountStatusEnum status)
        {
            RaiseEvent(new PaymentAccountStatusChanged(Id, status));
        }

        public void UpdateOnBoardingLink(string onBoardingLink)
        {
            RaiseEvent(new PaymentAccountOnBoardingLinkCreatedEvent(Id, onBoardingLink));
        }

        protected void Apply(PaymentAccountOnBoardingLinkCreatedEvent @event)
        {
            IntegrationInfo = IntegrationInfo with { OnBoardingLink = @event.OnBoardingLink };
        }

        protected void Apply(PaymentAccountCreatedEvent @event)
        {
            Id = @event.Id;
            Status = PaymentAccountStatusEnum.Pending;
            IntegrationInfo = new PaymentAccountIntegrationInfo(@event.IntegrationId, string.Empty);
        }

        protected void Apply(PaymentAccountStatusChanged @event)
        {
            Status = @event.Status;
        }
    }
}
