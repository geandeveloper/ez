using System.Threading;
using System.Threading.Tasks;
using EzCommon.EventHandlers;
using EzCommon.Infra.Storage;
using EzGym.Gyms;
using EzGym.Gyms.Events;
using EzGym.Gyms.Users;
using EzGym.Infra.Repository;
using EzGym.Infra.Storage;
using EzGym.Payments;
using EzGym.Wallets;

namespace EzGym.EventHandlers
{
    public class GymMemberShipRegisteredEventHandler : IEventHandler<GymMemberShipRegisteredEvent>
    {
        private readonly IGymRepository _repository;

        public GymMemberShipRegisteredEventHandler(IGymRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(GymMemberShipRegisteredEvent notification, CancellationToken cancellationToken)
        {
            var payment = await _repository.LoadAggregateAsync<Payment>(notification.GymMemberShip.PaymentId);

            var payerWallet= await _repository.QueryAsync<Wallet>(w => w.AccountId == notification.GymMemberShip.PayerAccountId);
            var receiverWallet = await _repository.QueryAsync<Wallet>(w => w.AccountId == notification.GymMemberShip.ReceiverAccountId);

            payerWallet.AddReceipt(payment.Id, true, payment.PaymentMethod, payment.PaymentStatus, payment.PaymentDateTime, payment.Value, payment.Description);

            receiverWallet.AddReceipt(payment.Id, true, payment.PaymentMethod, payment.PaymentStatus, payment.PaymentDateTime, payment.Value, payment.Description);

            payerWallet.AddReceipt(payment.Id, false, payment.PaymentMethod, payment.PaymentStatus, payment.PaymentDateTime, payment.Value, payment.Description);

            await _repository.SaveAggregateAsync(receiverWallet);
            await _repository.SaveAggregateAsync(payerWallet);
        }
    }
}
