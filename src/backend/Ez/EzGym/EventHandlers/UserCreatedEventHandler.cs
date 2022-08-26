using EzCommon.EventHandlers;
using EzGym.Accounts;
using EzIdentity.Events;
using System.Threading;
using System.Threading.Tasks;
using EzGym.Accounts.CreateAccount;

namespace EzGym.EventHandlers
{
    public class UserCreatedEventHandler : IEventHandler<UserCreatedEvent>
    {
        private readonly CreateAccountCommandHandler _handler;

        public UserCreatedEventHandler(CreateAccountCommandHandler handler)
        {
            _handler = handler;
        }

        public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            var accountStream = await _handler.Handle(new CreateAccountCommand(notification.Id, notification.UserName, AccountTypeEnum.User, true), cancellationToken);
        }
    }
}
