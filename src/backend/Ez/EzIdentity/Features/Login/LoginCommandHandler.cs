using EzCommon.CommandHandlers;
using EzCommon.Infra.Security;
using EzCommon.Infra.Storage;
using EzCommon.Models;
using EzIdentity.Models;
using System.Threading;
using System.Threading.Tasks;

namespace EzIdentity.Features.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand>
{
    private readonly IEventStore _eventStore;

    public LoginCommandHandler(
        IEventStore eventStore
        )
    {
        _eventStore = eventStore;
    }

    public async Task<EventStream> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = _eventStore.GetSnapShot<User>(user => user.Email == request.Email && user.Password == CryptographyService.CreateHash(request.Password));

        user.Login();

        return await _eventStore.SaveAsync(user);
    }
}
