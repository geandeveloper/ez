using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Storage;
using EzGym.Models;
using System.Threading;
using System.Threading.Tasks;

namespace EzGym.Features.Gyms.CreateGym
{
    public class CreateGymCommandHandler : ICommandHandler<CreateGymCommand>
    {

        private readonly IGymEventStore _eventStore;

        public CreateGymCommandHandler(IGymEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public Task<EventStream> Handle(CreateGymCommand request, CancellationToken cancellationToken)
        {
            throw new System.Exception();
        }
    }
}
