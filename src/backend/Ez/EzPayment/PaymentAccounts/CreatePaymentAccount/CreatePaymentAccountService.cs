using System.Threading.Tasks;
using EzPayment.Integrations.Gateways;
using Stripe;

namespace EzPayment.PaymentAccounts.CreatePaymentAccount
{
    public class CreatePaymentAccountService
    {
        private readonly PaymentGatewayFactory _paymentGatewayFactory;

        public CreatePaymentAccountService(PaymentGatewayFactory paymentGatewayFactory)
        {
            _paymentGatewayFactory = paymentGatewayFactory;
        }

        public Task<string> CreateAccountIntegrationAsync(string accountName, string mcc, string profileUrl)
        {
            var accountOptions = new AccountCreateOptions
            {
                Country = "BR",
                Type = "express",
                Capabilities = new AccountCapabilitiesOptions
                {
                    CardPayments = new AccountCapabilitiesCardPaymentsOptions { Requested = true },
                    Transfers = new AccountCapabilitiesTransfersOptions { Requested = true }
                },
                BusinessProfile = new AccountBusinessProfileOptions
                {
                    Name = accountName,
                    Mcc = mcc,
                    Url = profileUrl.ToLower()
                }
            };

            var account = _paymentGatewayFactory.UseStripePayment(gateway => gateway.CreateAccount(accountOptions));

            return Task.FromResult(account.Id);
        }

    }
}
