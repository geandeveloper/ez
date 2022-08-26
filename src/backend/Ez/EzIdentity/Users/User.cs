using EzCommon.Models;
using EzIdentity.Events;
using System;
using EzIdentity.Users.CreateUser;

namespace EzIdentity.Users
{
    public class User : AggregateRoot
    {
        public string UserName { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public RefreshToken RefreshToken { get; private set; }
        public string Password { get; private set; }
        public bool Activated { get; private set; }

        public User() { }

        public User(CreateUserCommand command)
        {
            RaiseEvent(new UserCreatedEvent(GenerateNewId(), command.Name, command.UserName, command.Email, command.Password));
        }

        public void Login(AccessToken accessTokenModel, RefreshToken refreshToken)
        {

            RaiseEvent(new SuccessLoginEvent(accessTokenModel, refreshToken));
        }

        public void RenewToken(AccessToken accessTokenModel, RefreshToken refreshToken)
        {
            RaiseEvent(new SucessRenewTokenEvent(accessTokenModel, refreshToken));
        }

        public void RevokeToken(string refreshToken)
        {
            if (RefreshToken.Value != refreshToken)
                throw new Exception("invalid token");

            RaiseEvent(new SucessRevokeTokenEvent());
        }

        protected void Apply(SucessRevokeTokenEvent @event)
        {
            RefreshToken = null;
        }

        public void Apply(SuccessLoginEvent @event)
        {
            RefreshToken = @event.RefreshToken;
        }

        public void Apply(SucessRenewTokenEvent @event)
        {
            RefreshToken = @event.RefreshToken;
        }

        public void Apply(UserCreatedEvent @event)
        {
            Id = @event.Id;
            Name = @event.Name;
            UserName = @event.UserName;
            Email = @event.Email;
            Password = @event.Password;
        }
    }
}
