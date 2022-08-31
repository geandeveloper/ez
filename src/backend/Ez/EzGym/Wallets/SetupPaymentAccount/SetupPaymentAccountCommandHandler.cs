using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Accounts;
using EzGym.Infra.Repository;
using EzPayment.Events.Accounts;
using EzPayment.PaymentAccounts;
using EzPayment.PaymentAccounts.CreatePaymentAccount;
using EzPayment.PaymentAccounts.CreatePaymentAccountSetupLink;
using EzPayment.PaymentAccounts.VerifyPaymentAccount;
using Microsoft.Extensions.Options;

namespace EzGym.Wallets.SetupPaymentAccount
{
    public class SetupPaymentAccountCommandHandler : ICommandHandler<SetupPaymentAccountCommand>
    {
        private readonly IGymRepository _gymRepository;
        private readonly CreatePaymentAccountCommandHandler _createPaymentAccount;
        private readonly CreatePaymentAccountSetupLinkCommandHandler _accountSetupLink;
        private readonly VerifyPaymentAccountCommandHandler _verifyPaymentAccountCommandHandler;
        private readonly IOptions<EzGymSettings> _settings;


        public SetupPaymentAccountCommandHandler(IGymRepository gymRepository, CreatePaymentAccountCommandHandler createPaymentAccount, CreatePaymentAccountSetupLinkCommandHandler accountSetupLink, IOptions<EzGymSettings> settings, VerifyPaymentAccountCommandHandler verifyPaymentAccountCommandHandler)
        {
            _gymRepository = gymRepository;
            _createPaymentAccount = createPaymentAccount;
            _accountSetupLink = accountSetupLink;
            _settings = settings;
            _verifyPaymentAccountCommandHandler = verifyPaymentAccountCommandHandler;
        }

        public async Task<EventStream> Handle(SetupPaymentAccountCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _gymRepository.QueryAsync<Wallet>(a => a.Id == request.WalletId);
            var account = await _gymRepository.QueryAsync<Account>(a => a.Id == wallet.AccountId);

            if (wallet.PaymentAccount == null)
            {
                var profileUrl = $"{_settings.Value.Frontend.EzGymProductionWebUrl}/{account.AccountName}";
                var paymentAccountEventStream = await _createPaymentAccount.Handle(new CreatePaymentAccountCommand(account.AccountName, profileUrl, MerchantCategoryCodes.ClubsAndGyms), cancellationToken);
                var paymentAccountEvent = paymentAccountEventStream.GetEvent<PaymentAccountCreatedEvent>();

                var newUrl = await GetPaymentAccountLinkAsync(paymentAccountEvent.Id, request.RefreshUrl, request.ReturnUrl, cancellationToken);

                wallet.UpdatePaymentAccount(paymentAccountEvent.Id, newUrl, PaymentAccountStatusEnum.Pending);
                return await _gymRepository.SaveAggregateAsync(wallet);
            }

            var verifyPaymentAccountStream = await _verifyPaymentAccountCommandHandler.Handle(new VerifyPaymentAccountCommand(wallet.PaymentAccount.Id), cancellationToken);
            var verifyPaymentAccountEvent = verifyPaymentAccountStream.GetEvent<PaymentAccountStatusChanged>();

            var updateUrl = await GetPaymentAccountLinkAsync(wallet.PaymentAccount.Id, request.RefreshUrl, request.ReturnUrl, cancellationToken);

            wallet.UpdatePaymentAccount(wallet.PaymentAccount.Id, updateUrl, verifyPaymentAccountEvent.Status);

            return await _gymRepository.SaveAggregateAsync(wallet);
        }

        private async Task<string> GetPaymentAccountLinkAsync(string paymentAccountId, string refreshUrl, string returnUrl, CancellationToken cancellationToken)
        {
            var paymentAccountLinkStream = await _accountSetupLink
                .Handle(new CreatePaymentAccountSetupLinkCommand(paymentAccountId, refreshUrl, returnUrl), cancellationToken);
            var paymentAccountLinkEvent = paymentAccountLinkStream.GetEvent<PaymentAccountOnBoardingLinkCreatedEvent>();
            return paymentAccountLinkEvent.OnBoardingLink;
        }
    }
}
