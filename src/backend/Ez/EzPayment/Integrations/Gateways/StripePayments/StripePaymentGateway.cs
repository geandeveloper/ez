using Stripe;
using Stripe.Checkout;

namespace EzPayment.Integrations.Gateways.StripePayments;

public class StripePaymentGateway : IStripePaymentGateway
{
    private readonly SessionService _sessionService;
    private readonly PaymentIntentService _paymentIntentService;
    private readonly AccountService _accountService;
    private readonly AccountLinkService _accountLinkService;

    public StripePaymentGateway(
        SessionService sessionService,
        PaymentIntentService paymentIntentService,
        AccountService accountService,
        AccountLinkService accountLinkService)
    {
        _sessionService = sessionService;
        _paymentIntentService = paymentIntentService;
        _accountService = accountService;
        _accountLinkService = accountLinkService;
    }

    public Account CreateAccount(AccountCreateOptions options)
    {
        return _accountService.Create(options);
    }

    public Account GetAccount(string accountId)
    {
        return _accountService.Get(accountId);
    }

    public AccountLink CreateAccountLink(AccountLinkCreateOptions options)
    {
        return _accountLinkService.Create(options);
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