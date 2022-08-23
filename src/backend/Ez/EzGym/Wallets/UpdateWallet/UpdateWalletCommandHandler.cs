using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Accounts;
using EzGym.Infra.Storage;

namespace EzGym.Wallets.UpdateWallet
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
            var account = await _eventStore.LoadAggregateAsync<Wallet>(request.AccountId);

            return await _eventStore.SaveAggregateAsync(account);
        }
    }
}
