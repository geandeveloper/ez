using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Commands;
using EzCommon.Models;
using EzPayment.Infra.Repository;

namespace EzPayment.Payments.PaymentReceived
{
    public record PaymentReceivedCommand(string IntegrationId, long Amount) : ICommand;

    public class PaymentReceivedCommandHandler : ICommandHandler<PaymentReceivedCommand>
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentReceivedCommandHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<EventStream> Handle(PaymentReceivedCommand request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.QueryAsync<Payment>(p => p.IntegrationId == request.IntegrationId);

            payment.Receive(request.Amount);

            return await _paymentRepository.SaveAggregateAsync(payment);
        }
    }
}
