using EzCommon.EventHandlers;
using EzGym.Accounts;
using System.Threading;
using System.Threading.Tasks;
using EzGym.Accounts.CreateAccount;
using EzIdentity.Events.Users;

namespace EzGym.EventHandlers
{
    public class UserEventHandler : IEventHandler<UserCreatedEvent>
    {
        private readonly CreateAccountCommandHandler _handler;

        public UserEventHandler(CreateAccountCommandHandler handler)
        {
            _handler = handler;
        }

        public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            var accountStream = await _handler.Handle(new CreateAccountCommand(notification.Id, notification.UserName, AccountTypeEnum.User, true), cancellationToken);
        }
    }
}
