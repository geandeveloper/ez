using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Infra.Security;
using EzCommon.Models;
using EzIdentity.Infra.Repository;

namespace EzIdentity.Users.CreateUser
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
    {
        private readonly IIdentityRepository _repository;
        public CreateUserCommandHandler(IIdentityRepository repository)
        {
            _repository = repository;
        }

        public async Task<EventStream> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User(request with { Password = CryptographyService.CreateHash(request.Password) });
            return await _repository.SaveAggregateAsync(user);
        }
    }
}
