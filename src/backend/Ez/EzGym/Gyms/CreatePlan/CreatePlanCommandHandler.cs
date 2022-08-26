using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Repository;
using EzGym.Infra.Storage;

namespace EzGym.Gyms.CreatePlan
{
    public record CreatePlanCommandHandler : ICommandHandler<CreatePlanCommand>
    {

        private readonly IGymRepository _repository;

        public CreatePlanCommandHandler(IGymRepository repository)
        {
            _repository = repository;
        }

        public async Task<EventStream> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
        {
            var gym = await _repository.LoadAggregateAsync<Gym>(request.GymId);
            gym.CreatePlan(request);

            return await _repository.SaveAggregateAsync(gym);
        }
    }
}
