using Stripe;
using Stripe.Checkout;

namespace EzPayment.Integrations.Gateways.StripePayments
{
    public interface IStripePaymentGateway
    {
        Session CreateCheckoutSession(SessionCreateOptions options);
        PaymentIntent CreatePaymentIntent(PaymentIntentCreateOptions options);
    }
}
