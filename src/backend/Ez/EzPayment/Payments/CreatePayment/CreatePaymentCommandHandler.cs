using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Commands;
using EzCommon.Models;
using EzPayment.Infra.Repository;
using EzPayment.Integrations.Gateways;
using Stripe;

namespace EzPayment.Payments.CreatePayment
{

    public record CreatePaymentCommand(long Amount, string Description) : ICommand;

    public class CreatePaymentCommandHandler : ICommandHandler<CreatePaymentCommand>
    {
        private readonly GatewayFactory _gatewayFactory;
        private readonly IPaymentRepository _paymentRepository;

        public CreatePaymentCommandHandler(GatewayFactory gatewayFactory, IPaymentRepository paymentRepository)
        {
            _gatewayFactory = gatewayFactory;
            _paymentRepository = paymentRepository;
        }


        public Task<EventStream> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var (amount, description) = request;


            var options = new PaymentIntentCreateOptions
            {
                Amount = request.Amount,
                Currency = "usd",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            };

            var paymentIntent = _gatewayFactory.UseStripePayment(stripe => stripe.CreatePaymentIntent(options));

            var payment = new Payment(paymentIntent.Id, new CreatePix.CreatePaymentCommand(amount, description));

            return _paymentRepository.SaveAggregateAsync(payment);
        }
    }
}
