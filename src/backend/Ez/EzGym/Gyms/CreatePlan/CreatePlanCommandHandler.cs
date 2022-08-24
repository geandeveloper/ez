using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Storage;

namespace EzGym.Gyms.CreatePlan
{
    public record CreatePlanCommandHandler : ICommandHandler<CreatePlanCommand>
    {

        private readonly IGymEventStore _eventStore;

        public CreatePlanCommandHandler(IGymEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<EventStream> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
        {
            var gym = await _eventStore.LoadAggregateAsync<Gym>(request.GymId);
            gym.CreatePlan(request);

            return await _eventStore.SaveAggregateAsync(gym);
        }
    }
}
