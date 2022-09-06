using System.Threading;
using System.Threading.Tasks;
using EzCommon.EventHandlers;
using EzGym.Events.Gym;
using EzGym.Gyms;
using EzGym.Infra.Repository;
using EzGym.Wallets;
using EzPayment.Payments;

namespace EzGym.EventHandlers
{
    public class GymMemberEventHandler :
        IEventHandler<GymMemberShipCreatedEvent>,
        IEventHandler<GymMemberShipPaidEvent>
    {
        private readonly IGymRepository _repository;

        public GymMemberEventHandler(IGymRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(GymMemberShipCreatedEvent notification, CancellationToken cancellationToken)
        {
            var payment = await _repository.QueryAsync<GymMemberShip>(p => p.PaymentId == notification.PaymentId);
            var payerWallet = await _repository.QueryAsync<Wallet>(w => w.AccountId == payment.PayerAccountId);

            var walletReceipt = new WalletReceipt(payerWallet.Id, payment.PaymentId, PaymentStatusEnum.Pending,
                payment.PaymentDateTime, payment.Amount, 0, "Saldo Adicionado");

            await _repository.SaveAggregateAsync(walletReceipt);
        }

        public async Task Handle(GymMemberShipPaidEvent notification, CancellationToken cancellationToken)
        {
            var memberShip = await _repository.LoadAggregateAsync<GymMemberShip>(notification.Id);
            var payerWallet = await _repository.QueryAsync<Wallet>(w => w.AccountId == memberShip.PayerAccountId);
            var receiverWallet = await _repository.QueryAsync<Wallet>(w => w.AccountId == memberShip.ReceiverAccountId);

            var payerReceipt = await _repository.QueryAsync<WalletReceipt>(w => w.WalletId == payerWallet.Id && w.PaymentId == memberShip.PaymentId);
            payerReceipt.UpdateReceipt(PaymentStatusEnum.Approved, notification.PaymentDateTime);


            var payerPaymentReceipt = new WalletReceipt(payerWallet.Id, memberShip.PaymentId, PaymentStatusEnum.Approved, notification.PaymentDateTime, -memberShip.Amount, 0, $"Pagamento efetuado, plano {memberShip.PaymentId}");
            var receiverPaymentReceipt = new WalletReceipt(receiverWallet.Id, memberShip.PaymentId, PaymentStatusEnum.Approved, notification.PaymentDateTime, memberShip.Amount, memberShip.ApplicationFeeAmount, $"Pagamento recebido, plano {memberShip.PlanId}");

            await _repository.SaveAggregateAsync(payerReceipt);
            await _repository.SaveAggregateAsync(payerPaymentReceipt);
            await _repository.SaveAggregateAsync(receiverPaymentReceipt);
        }
    }
}
