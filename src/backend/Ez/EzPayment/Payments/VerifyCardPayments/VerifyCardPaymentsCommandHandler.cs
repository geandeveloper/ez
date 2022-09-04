using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Commands;
using EzCommon.Models;
using EzPayment.Infra.Repository;
using EzPayment.Integrations.Gateways;

namespace EzPayment.Payments.VerifyCardPayments
{
    public record VerifyCardPaymentsCommand(string PaymentId) : ICommand;

    public class VerifyCardPaymentsCommandHandler : ICommandHandler<VerifyCardPaymentsCommand>
    {
        private readonly PaymentGatewayFactory _paymentGatewayFactory;
        private readonly IPaymentRepository _paymentRepository;

        public VerifyCardPaymentsCommandHandler(PaymentGatewayFactory paymentGatewayFactory, IPaymentRepository paymentRepository)
        {
            _paymentGatewayFactory = paymentGatewayFactory;
            _paymentRepository = paymentRepository;
        }

        public async Task<EventStream> Handle(VerifyCardPaymentsCommand request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.LoadAggregateAsync<Payment>(request.PaymentId);
            var paymentIntent = _paymentGatewayFactory.UseStripePayment(stripe => stripe.GetPaymentIntent(payment.CardInfo.IntegrationId));

            if (paymentIntent.Status != "succeeded") return payment.ToEventStream();

            payment.Receive(payment.Amount);
            return await _paymentRepository.SaveAggregateAsync(payment);

        }
    }
}
