using EzCommon.CommandHandlers;
using EzCommon.Infra.Security;
using EzCommon.Infra.Storage;
using EzCommon.Models;
using EzIdentity.Infra.Storage;
using EzIdentity.Models;
using EzIdentity.SnapShots;
using System.Threading;
using System.Threading.Tasks;

namespace EzIdentity.Features.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand>
{
    private readonly IEventStore _eventStore;

    public LoginCommandHandler(
        IIdentityEventStore eventStore
        )
    {
        _eventStore = eventStore;
    }

    public async Task<EventStream> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _eventStore.QueryLatestVersionAsync<UserSnapShot, User>(user => user.UserName == request.UserName && user.Password == CryptographyService.CreateHash(request.Password));
        user.Login();
        return await _eventStore.SaveAsync<User, UserSnapShot>(user);
    }
}
