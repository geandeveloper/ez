using EzCommon.Models;
using EzIdentity.Events;
using EzIdentity.Features.CreateUser;
using EzIdentity.Features.Login;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace EzIdentity.Models
{
    public class User : AggregateRoot
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public RefreshToken RefreshToken { get; private set; }
        public string Password { get; private set; }
        public bool Activated { get; private set; }

        private User() { }

        public User(CreateUserCommand command)
        {
            RaiseEvent(new UserCreatedEvent(Guid.NewGuid(), command.Name, command.Email, command.Password));
        }

        public void SuccessLogin(AccessToken accessToken, RefreshToken refreshToken)
        {
            RaiseEvent(new SucessLoginEvent(accessToken, refreshToken));
        }

        public void SuccessRefreshToken(AccessToken accessToken, RefreshToken refreshToken)
        {
            RaiseEvent(new SucessRenewTokenEvent(accessToken, refreshToken));
        }

        protected override void RegisterEvents()
        {
            RegisterEvent<UserCreatedEvent>(When);
            RegisterEvent<SucessLoginEvent>(When);
            RegisterEvent<SucessRenewTokenEvent>(When);
        }

        private void When(SucessLoginEvent @event)
        {
            RefreshToken = @event.RefreshToken;
        }

        private void When(SucessRenewTokenEvent @event)
        {
            RefreshToken = @event.RefreshToken;
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
