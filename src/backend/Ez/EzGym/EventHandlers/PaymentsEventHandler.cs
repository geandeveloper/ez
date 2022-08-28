using System.Threading;
using System.Threading.Tasks;
using EzCommon.EventHandlers;
using EzGym.Gyms;
using EzGym.Infra.Repository;
using EzPayment.Events.Payments;

namespace EzGym.EventHandlers
{
    public class PaymentsEventHandler : IEventHandler<PaymentReceivedEvent>
    {
        private readonly IGymRepository _repository;

        public PaymentsEventHandler(IGymRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(PaymentReceivedEvent notification, CancellationToken cancellationToken)
        {
            var memberShip = await _repository.QueryAsync<GymMemberShip>(gm => gm.PaymentId == notification.PaymentId);
            memberShip.Paid();
            await _repository.SaveAggregateAsync(memberShip);
        }
    }
}
