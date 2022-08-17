using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Storage;
using EzGym.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EzGym.Features.Accounts.UpdateWallet
{
    public class UpdateWalletCommandHandler : ICommandHandler<UpdateWalletCommand>
    {

        private readonly IGymEventStore _eventStore;

        public UpdateWalletCommandHandler(IGymEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<EventStream> Handle(UpdateWalletCommand request, CancellationToken cancellationToken)
        {
            var account = await _eventStore.LoadAggregateAsync<Account>(request.AccountId);
            account.UpdateWallet(request.Pix);

            return await _eventStore.SaveAggregateAsync(account);
        }
    }
}
