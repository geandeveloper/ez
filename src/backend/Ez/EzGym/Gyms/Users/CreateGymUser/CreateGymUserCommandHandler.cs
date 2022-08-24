using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Commands;
using EzCommon.Models;
using EzGym.Infra.Storage;

namespace EzGym.Gyms.Users.CreateGymUser
{
    public record CreateGymUserCommand(string GymId, string Name, string Email) : ICommand;

    public class CreateGymUserCommandHandler : ICommandHandler<CreateGymUserCommand>
    {
        private readonly IGymEventStore _eventStore;

        public CreateGymUserCommandHandler(IGymEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<EventStream> Handle(CreateGymUserCommand request, CancellationToken cancellationToken)
        {
            var gymUser = new GymUser(request);

            return await _eventStore.SaveAggregateAsync(gymUser);
        }
    };
}
