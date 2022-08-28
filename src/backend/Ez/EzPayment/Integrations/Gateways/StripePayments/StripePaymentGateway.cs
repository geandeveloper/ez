using Stripe;
using Stripe.Checkout;

namespace EzPayment.Integrations.Gateways.StripePayments;

public class StripePaymentGateway : IStripePaymentGateway
{
    private readonly SessionService _sessionService;
    private readonly PaymentIntentService _paymentIntentService;

    public StripePaymentGateway(SessionService sessionService, PaymentIntentService paymentIntentService)
    {
        _sessionService = sessionService;
        _paymentIntentService = paymentIntentService;
    }

    public Session CreateCheckoutSession(SessionCreateOptions options)
    {
        return _sessionService.Create(options);
    }

    public PaymentIntent CreatePaymentIntent(PaymentIntentCreateOptions options)
    {
        return _paymentIntentService.Create(options);
    }

}