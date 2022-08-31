using System.Threading.Tasks;
using EzPayment.Integrations.Gateways;
using Stripe;

namespace EzPayment.PaymentAccounts.CreatePaymentAccountSetupLink
{
    public class CreatePaymentAccountSetupLinkService
    {
        private readonly PaymentGatewayFactory _paymentGatewayFactory;

        public CreatePaymentAccountSetupLinkService(PaymentGatewayFactory paymentGatewayFactory)
        {
            _paymentGatewayFactory = paymentGatewayFactory;
        }

        public Task<string> CreateAccountOnBoardingLinkIntegrationAsync(string integrationId, string refreshUrl, string returnUrl)
        {
            var options = new AccountLinkCreateOptions
            {
                Account = integrationId,
                RefreshUrl = refreshUrl,
                ReturnUrl = returnUrl,
                Type = "account_onboarding",
                Collect = "eventually_due"
            };

            var accountLink = _paymentGatewayFactory.UseStripePayment(gateway => gateway.CreateAccountLink(options));

            return Task.FromResult(accountLink.Url);
        }

    }
}
