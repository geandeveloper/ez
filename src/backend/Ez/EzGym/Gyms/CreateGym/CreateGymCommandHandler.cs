using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Repository;
using EzGym.Infra.Storage;

namespace EzGym.Gyms.CreateGym
{
    public class CreateGymCommandHandler : ICommandHandler<CreateGymCommand>
    {

        private readonly IGymRepository _repository;

        public CreateGymCommandHandler(IGymRepository repository)
        {
            _repository = repository;
        }

        public async Task<EventStream> Handle(CreateGymCommand request, CancellationToken cancellationToken)
        {
            var gym = new Gym(request);

            return await _repository.SaveAggregateAsync(gym);
        }
    }
}
