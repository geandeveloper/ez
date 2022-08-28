using System;
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
            var payment = await _repository.LoadAggregateAsync<Payment>(notification.PaymentId);

            var payerWallet = await _repository.QueryAsync<Wallet>(w => w.AccountId == notification.PayerAccountId);
            var receiverWallet = await _repository.QueryAsync<Wallet>(w => w.AccountId == notification.ReceiverAccountId);

            payerWallet.AddReceipt(payment.Id, true, payment.PaymentMethod, payment.PaymentStatus, payment.PaymentDateTime, payment.Amount, payment.Description);

            receiverWallet.AddReceipt(payment.Id, true, payment.PaymentMethod, payment.PaymentStatus, payment.PaymentDateTime, payment.Amount, payment.Description);

            payerWallet.AddReceipt(payment.Id, false, payment.PaymentMethod, payment.PaymentStatus, payment.PaymentDateTime, payment.Amount, payment.Description);

            await _repository.SaveAggregateAsync(receiverWallet);
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
                PaymentDateTime = notification.PaymentDateTime
            });

            receiverWallet.UpdateReceipt(memberShip.PaymentId, state => state with
            {
                PaymentStatus = PaymentStatusEnum.Approved,
                PaymentDateTime = notification.PaymentDateTime
            });

            await _repository.SaveAggregateAsync(receiverWallet);
            await _repository.SaveAggregateAsync(payerWallet);
        }
    }
}
