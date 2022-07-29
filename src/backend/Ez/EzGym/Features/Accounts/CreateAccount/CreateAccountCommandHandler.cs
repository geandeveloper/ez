using EzCommon.CommandHandlers;
using EzCommon.Infra.Storage;
using EzCommon.Models;
using EzGym.Infra.Storage;
using EzGym.Models;
using System.Threading;
using System.Threading.Tasks;

namespace EzGym.Features.Accounts.CreateAccount;

public class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand>
{

    private readonly IEventStore _eventStore;

    public CreateAccountCommandHandler(IGymEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public Task<EventStream> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = new Account(request);
        return _eventStore.SaveAsync(account);
    }
}
