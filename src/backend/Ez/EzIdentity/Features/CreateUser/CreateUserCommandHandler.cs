using EzCommon.CommandHandlers;
using EzCommon.Infra.Security;
using EzCommon.Infra.Storage;
using EzCommon.Models;
using EzIdentity.Models;
using System.Threading;
using System.Threading.Tasks;

namespace EzIdentity.Features.CreateUser
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
    {
        private readonly IEventStore _eventStore;
        public CreateUserCommandHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<EventStream> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User(request with { Password = CryptographyService.CreateHash(request.Password) });
            return await _eventStore.SaveAsync(user);
        }
    }
}
