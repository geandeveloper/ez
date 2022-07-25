using EzCommon.CommandHandlers;
using EzCommon.Infra.Storage;
using EzCommon.Models;
using EzGym.Models;
using System.Threading;
using System.Threading.Tasks;

namespace EzGym.Features.CreateGym
{
    public class CreateGymCommandHandler : ICommandHandler<CreateGymCommand>
    {

        private readonly IEventStore _eventStore;

        public CreateGymCommandHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public Task<EventStream> Handle(CreateGymCommand request, CancellationToken cancellationToken)
        {
            var gym = new Gym(request);
            return _eventStore.SaveAsync(gym);
        }
    }
}
