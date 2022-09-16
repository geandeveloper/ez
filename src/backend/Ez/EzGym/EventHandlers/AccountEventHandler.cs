using EzCommon.EventHandlers;
using EzGym.Accounts.Events;
using System.Threading;
using System.Threading.Tasks;
using EzGym.Accounts;
using EzGym.Gyms.CreateGym;
using EzGym.Players.CreatePlayer;

namespace EzGym.EventHandlers
{
    public class AccountEventHandler : 
        IEventHandler<AccountCreatedEvent>
    {
        private readonly CreateGymCommandHandler _createGym;
        private readonly CreatePlayerCommandHandler _createPlayer;

        public AccountEventHandler(CreateGymCommandHandler createGym, CreatePlayerCommandHandler createPlayer)
        {
            _createGym = createGym;
            _createPlayer = createPlayer;
        }

        public async Task Handle(AccountCreatedEvent notification, CancellationToken cancellationToken)
        {
            var createPlayerCommand = new CreatePlayerCommand(AccountId: notification.Id);
            await _createPlayer.Handle(createPlayerCommand, cancellationToken);

            if (notification.Command.AccountType != AccountTypeEnum.Gym)
                return;

            var createGymCommand = new CreateGymCommand(AccountId: notification.Id);
            await _createGym.Handle(createGymCommand, cancellationToken);

        }
    }
}
