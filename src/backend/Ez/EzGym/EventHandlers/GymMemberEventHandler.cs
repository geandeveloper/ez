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
            payerWallet.AddReceipt(payment.PaymentId, PaymentStatusEnum.Pending, payment.PaymentDateTime, payment.Amount, 0, "Saldo adicionado");
            await _repository.SaveAggregateAsync(payerWallet);
        }

        public async Task Handle(GymMemberShipPaidEvent notification, CancellationToken cancellationToken)
        {
            var memberShip = await _repository.LoadAggregateAsync<GymMemberShip>(notification.Id);
            var payerWallet = await _repository.QueryAsync<Wallet>(w => w.AccountId == memberShip.PayerAccountId);
            var receiverWallet = await _repository.QueryAsync<Wallet>(w => w.AccountId == memberShip.ReceiverAccountId);

            payerWallet.UpdateReceipt(memberShip.PaymentId, state => state with
            {
                PaymentStatus = PaymentStatusEnum.Approved,
                PaymentDateTime = notification.PaymentDateTime,
            });

            payerWallet.AddReceipt(memberShip.PaymentId, PaymentStatusEnum.Approved, notification.PaymentDateTime, -memberShip.Amount, 0, $"Pagamento efetuado, plano {memberShip.PaymentId}");
            receiverWallet.AddReceipt(memberShip.PaymentId, PaymentStatusEnum.Approved, notification.PaymentDateTime, memberShip.Amount, memberShip.ApplicationFeeAmount, $"Pagamento recebido, plano {memberShip.PlanId}");

            await _repository.SaveAggregateAsync(receiverWallet);
            await _repository.SaveAggregateAsync(payerWallet);
        }
    }
}
