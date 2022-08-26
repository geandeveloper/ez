using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Infra.Storage;
using EzCommon.Models;
using EzGym.Infra.Repository;
using EzGym.Infra.Storage;
using EzGym.Wallets;

namespace EzGym.Accounts.CreateAccount;

public class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand>
{

    private readonly IGymRepository _repository;

    public CreateAccountCommandHandler(IGymRepository repository)
    {
        _repository = repository;
    }

    public async Task<EventStream> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = new Account(request);
        var wallet = new Wallet(account.Id);

        await _repository.SaveAggregateAsync(wallet);

        return await _repository.SaveAggregateAsync(account);
    }
}
