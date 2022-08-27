using System.Threading;
using System.Threading.Tasks;
using EzCommon.CommandHandlers;
using EzCommon.Models;
using EzGym.Infra.Repository;

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
            var wallet = await _repository.LoadAggregateAsync<Wallet>(request.AccountId);
            wallet.UpdateWallet(request.Pix);

            return await _repository.SaveAggregateAsync(wallet);
        }
    }
}
