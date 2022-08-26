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
            var gym = await _repository.QueryAsync<Gym>(g => g.Id == notification.GymMemberShip.GymId);
            var gymUser = await _repository.QueryAsync<GymUser>(g => g.Id == notification.GymMemberShip.GymUserId);

            var gymWallet = await _repository.QueryAsync<Wallet>(w => w.AccountId == gym.AccountId);
            var userWallet = await _repository.QueryAsync<Wallet>(w => w.AccountId == gymUser.AccountId);

            userWallet.AddReceipt(payment.Id, true, payment.PaymentMethod, payment.PaymentStatus, payment.PaymentDateTime, payment.Value, payment.Description);

            gymWallet.AddReceipt(payment.Id, true, payment.PaymentMethod, payment.PaymentStatus, payment.PaymentDateTime, payment.Value, payment.Description);

            userWallet.AddReceipt(payment.Id, false, payment.PaymentMethod, payment.PaymentStatus, payment.PaymentDateTime, payment.Value, payment.Description);

            await _repository.SaveAggregateAsync(gymWallet);
            await _repository.SaveAggregateAsync(userWallet);
        }
    }
}
