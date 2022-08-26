using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Commands;
using EzCommon.Models;
using EzGym.Infra.Repository;
using EzGym.Infra.Storage;

namespace EzGym.Gyms.Users.CreateGymUser
{
    public record CreateGymUserCommand(string GymId,string Name, string Email) : ICommand;

    public class CreateGymUserCommandHandler : ICommandHandler<CreateGymUserCommand>
    {
        private readonly IGymRepository _repository;

        public CreateGymUserCommandHandler(IGymRepository repository)
        {
            _repository = repository;
        }

        public async Task<EventStream> Handle(CreateGymUserCommand request, CancellationToken cancellationToken)
        {
            var gymUser = new GymUser(request);

            return await _repository.SaveAggregateAsync(gymUser);
        }
    };
}
