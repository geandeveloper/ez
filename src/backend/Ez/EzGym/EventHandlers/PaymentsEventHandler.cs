using System.Threading;
using System.Threading.Tasks;
using EzCommon.EventHandlers;
using EzGym.Gyms;
using EzPayment.Events.Payments;
using EzPayment.Infra.Repository;

namespace EzGym.EventHandlers
{
    public class PaymentsEventHandler : IEventHandler<PaymentReceivedEvent>
    {
        private readonly IPaymentRepository _repository;

        public PaymentsEventHandler(IPaymentRepository repository)
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
