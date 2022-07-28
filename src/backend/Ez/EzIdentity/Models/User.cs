using EzCommon.Models;
using EzIdentity.Events;
using EzIdentity.Features.CreateUser;
using EzIdentity.Services;
using System;
using System.Security.Claims;

namespace EzIdentity.Models
{
    public class User : AggregateRoot
    {
        public string UserName { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public RefreshToken RefreshToken { get; private set; }
        public string Password { get; private set; }
        public bool Activated { get; private set; }

        private User() { }

        public User(CreateUserCommand command)
        {
            RaiseEvent(new UserCreatedEvent(Guid.NewGuid(), command.Name, command.UserName, command.Email, command.Password));
        }

        public void Login()
        {
            var accessToken = TokenService.GenerateAccessToken(() => new Claim[]
            {
                    new Claim(nameof(Id), Id.ToString()),
                    new Claim(ClaimTypes.Email, Email),
                    new Claim(ClaimTypes.NameIdentifier, UserName),
                    new Claim(ClaimTypes.Name, Name ?? UserName)
            });

            var refreshToken = TokenService.GenereateRefreshToken();

            RaiseEvent(new SucessLoginEvent(accessToken, refreshToken));
        }

        public void RenewToken()
        {
            if (RefreshToken.Expires <= DateTime.UtcNow)
                throw new Exception("Refresh token already expired, please login again");

            var accessToken = TokenService.GenerateAccessToken(() => new Claim[]
                {
                    new Claim(nameof(Id), Id.ToString()),
                    new Claim(ClaimTypes.Email, Email),
                    new Claim(ClaimTypes.NameIdentifier, UserName),
                    new Claim(ClaimTypes.Name, Name ?? UserName)
                });

            var refreshToken = TokenService.GenereateRefreshToken();

            RaiseEvent(new SucessRenewTokenEvent(accessToken, refreshToken));
        }

        public void RevokeToken(string refreshToken)
        {
            if (RefreshToken.Value != refreshToken)
                throw new Exception("invalid token");

            RaiseEvent(new SucessRevokeTokenEvent());
        }

        protected override void RegisterEvents()
        {
            RegisterEvent<UserCreatedEvent>(When);
            RegisterEvent<SucessLoginEvent>(When);
            RegisterEvent<SucessRenewTokenEvent>(When);
            RegisterEvent<SucessRevokeTokenEvent>(When);
        }

        private void When(SucessRevokeTokenEvent obj)
        {
            RefreshToken = null;
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
            UserName = @event.UserName;
            Email = @event.Email;
            Password = @event.Password;
        }
    }
}
