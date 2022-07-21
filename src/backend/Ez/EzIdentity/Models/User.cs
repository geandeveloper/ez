using EzCommon.Infra.Security;
using EzCommon.Models;
using EzIdentity.Events;
using EzIdentity.Features.CreateUser;
using EzIdentity.Features.Login;
using System;

namespace EzIdentity.Models
{
    public class User : AggregateRoot
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public bool Activated { get; private set; }

        private User() { }

        public User(CreateUserCommand command)
        {
            RaiseEvent(new UserCreatedEvent(Guid.NewGuid(), command.Name, command.Email, command.Password));
        }

        public void SuccessLogin(LoginCommand command, Token token)
        {
            RaiseEvent(new SucessLoginEvent(token.AccessToken));
        }

        protected override void RegisterEvents()
        {
            RegisterEvent<UserCreatedEvent>(When);
            RegisterEvent<SucessLoginEvent>(When);
        }

        private void When(SucessLoginEvent @event)
        {
        }

        private void When(UserCreatedEvent @event)
        {
            Id = @event.Id;
            Name = @event.Name;
            Email = @event.Email;
            Password = @event.Password;
        }
    }
}
