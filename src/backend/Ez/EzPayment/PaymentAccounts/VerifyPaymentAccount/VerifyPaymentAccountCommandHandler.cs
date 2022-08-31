using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Commands;
using EzCommon.Models;
using EzPayment.Infra.Repository;
using EzPayment.Integrations.Gateways;

namespace EzPayment.PaymentAccounts.VerifyPaymentAccount
{
    public record VerifyPaymentAccountCommand(string PaymentAccountId) : ICommand;
    public class VerifyPaymentAccountCommandHandler : ICommandHandler<VerifyPaymentAccountCommand>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly PaymentGatewayFactory _paymentGatewayFactory;

        public VerifyPaymentAccountCommandHandler(IPaymentRepository paymentRepository, PaymentGatewayFactory paymentGatewayFactory)
        {
            _paymentRepository = paymentRepository;
            _paymentGatewayFactory = paymentGatewayFactory;
        }

        public async Task<EventStream> Handle(VerifyPaymentAccountCommand request, CancellationToken cancellationToken)
        {
            var paymentAccount = await _paymentRepository.LoadAggregateAsync<PaymentAccount>(request.PaymentAccountId);
            var paymentAccountIntegration = _paymentGatewayFactory.UseStripePayment(s => s.GetAccount(paymentAccount.IntegrationInfo.Id));

            var paymentAccountStatus = paymentAccountIntegration.PayoutsEnabled
                ? PaymentAccountStatusEnum.Approved
                : PaymentAccountStatusEnum.Pending;

            paymentAccount.UpdateStatus(paymentAccountStatus);
            return await _paymentRepository.SaveAggregateAsync(paymentAccount);
        }
    }
}
