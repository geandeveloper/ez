using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzPayment.Infra.Repository;

namespace EzPayment.PaymentAccounts.CreatePaymentAccount
{
    public class CreatePaymentAccountCommandHandler : ICommandHandler<CreatePaymentAccountCommand>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly CreatePaymentAccountService _createPaymentAccountService;

        public CreatePaymentAccountCommandHandler(IPaymentRepository paymentRepository, CreatePaymentAccountService createPaymentAccountService)
        {
            _paymentRepository = paymentRepository;
            _createPaymentAccountService = createPaymentAccountService;
        }

        public async Task<EventStream> Handle(CreatePaymentAccountCommand request, CancellationToken cancellationToken)
        {
            var accountIntegrationId = await _createPaymentAccountService.CreateAccountIntegrationAsync(request.AccountName, request.Mcc, request.ProfileUrl);
            var account = new PaymentAccount(accountIntegrationId);

            return await _paymentRepository.SaveAggregateAsync(account);
        }
    }
}
