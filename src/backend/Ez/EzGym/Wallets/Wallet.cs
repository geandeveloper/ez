using EzCommon.Models;
using EzGym.Events.Wallet;
using EzPayment.PaymentAccounts;

namespace EzGym.Wallets
{
    public class Wallet : AggregateRoot
    {
        public string AccountId { get; private set; }
        public Pix Pix { get; private set; }
        public PaymentAccount PaymentAccount { get; private set; }

        public Wallet()
        {
        }

        public Wallet(string accountId)
        {
            RaiseEvent(new WalletCreatedEvent(GenerateNewId(), accountId));
        }

        public void UpdateWallet(Pix pix)
        {
            RaiseEvent(new WalletUpdatedEvent(Id, pix));
        }

        public void UpdatePaymentAccount(string paymentAccountId, string onBoardingLink,
            PaymentAccountStatusEnum paymentAccountStatus)
        {
            RaiseEvent(new PaymentAccountChangedEvent(Id, paymentAccountId, onBoardingLink, paymentAccountStatus));
        }

        protected void Apply(PaymentAccountChangedEvent @event)
        {
            PaymentAccount = new PaymentAccount(@event.PaymentAccountId, @event.OnBoardingLink,
                @event.PaymentAccountStatus);
        }

        protected void Apply(WalletCreatedEvent @event)
        {
            Id = @event.Id;
            AccountId = @event.AccountId;
            PaymentAccount = null;
        }

        protected void Apply(WalletUpdatedEvent @event)
        {
            Pix = @event.Pix;
        }
    }
}
