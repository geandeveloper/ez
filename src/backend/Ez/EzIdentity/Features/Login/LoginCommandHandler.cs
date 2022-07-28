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
    private readonly IQueryStorage _queryStorage;

    public LoginCommandHandler(
        IEventStore eventStore,
        IQueryStorage queryStorage 
        )
    {
        _eventStore = eventStore;
        _queryStorage = queryStorage;
    }

    public async Task<EventStream> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = _queryStorage.GetSnapShot<User>(user => user.Email == request.Email && user.Password == CryptographyService.CreateHash(request.Password));

        user.Login();

        return await _eventStore.SaveAsync(user);
    }
}
