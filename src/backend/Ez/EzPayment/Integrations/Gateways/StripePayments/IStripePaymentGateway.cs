using Stripe;
using Stripe.Checkout;

namespace EzPayment.Integrations.Gateways.StripePayments
{
    public interface IStripePaymentGateway
    {
        Account CreateAccount(AccountCreateOptions options);
        Account GetAccount(string accountId);
        AccountLink CreateAccountLink(AccountLinkCreateOptions options);
        Session CreateCheckoutSession(SessionCreateOptions options);
        PaymentIntent CreatePaymentIntent(PaymentIntentCreateOptions options);
        PaymentIntent GetPaymentIntent(string paymentId);
    }
}
