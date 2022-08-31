using EzCommon.EventHandlers;
using EzGym.Accounts.Events;
using System.Threading;
using System.Threading.Tasks;
using EzGym.Accounts;
using EzGym.Gyms.CreateGym;

namespace EzGym.EventHandlers
{
    public class AccountEventHandler : IEventHandler<AccountCreatedEvent>
    {
        private readonly CreateGymCommandHandler _handler;

        public AccountEventHandler(CreateGymCommandHandler handler)
        {
            _handler = handler;
        }

        public async Task Handle(AccountCreatedEvent notification, CancellationToken cancellationToken)
        {
            if (notification.Command.AccountType != AccountTypeEnum.Gym)
                return;

            var createGymCommand = new CreateGymCommand(AccountId: notification.Id);
            await _handler.Handle(createGymCommand, cancellationToken);
        }
    }
}
