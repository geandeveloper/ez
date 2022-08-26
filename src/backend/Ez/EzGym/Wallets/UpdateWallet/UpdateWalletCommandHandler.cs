using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Repository;
using EzGym.Infra.Storage;

namespace EzGym.Wallets.UpdateWallet
{
    public class UpdateWalletCommandHandler : ICommandHandler<UpdateWalletCommand>
    {

        private readonly IGymRepository _repository;

        public UpdateWalletCommandHandler(IGymRepository repository)
        {
            _repository = repository;
        }

        public async Task<EventStream> Handle(UpdateWalletCommand request, CancellationToken cancellationToken)
        {
            var account = await _repository.LoadAggregateAsync<Wallet>(request.AccountId);

            return await _repository.SaveAggregateAsync(account);
        }
    }
}
