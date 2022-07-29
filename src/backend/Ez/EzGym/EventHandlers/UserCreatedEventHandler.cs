using EzCommon.EventHandlers;
using EzGym.Features.Accounts.CreateAccount;
using EzIdentity.Events;
using System.Threading;
using System.Threading.Tasks;

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
            await _handler.Handle(new CreateAccountCommand(notification.Id, notification.UserName, Models.AccountTypeEnum.User,true), cancellationToken);
        }
    }
}
