using EzCommon.EventHandlers;
using EzGym.Accounts.Events;
using System.Threading;
using System.Threading.Tasks;
using EzGym.Gyms.CreateGym;

namespace EzGym.EventHandlers
{
    public class AccountCreatedEventHandler : IEventHandler<AccountCreatedEvent>
    {
        private readonly CreateGymCommandHandler _handler;

        public AccountCreatedEventHandler(CreateGymCommandHandler handler)
        {
            _handler = handler;
        }

        public async Task Handle(AccountCreatedEvent notification, CancellationToken cancellationToken)
        {
            var command = new CreateGymCommand(AccountId: notification.Id);
            await _handler.Handle(command, cancellationToken);
        }
    }
}
