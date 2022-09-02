using EzGym.Accounts.Events;

namespace EzGym.Projections
{
    public class SearchAccountsProjection
    {
        public string Id { get; set; }
        public string AccountName { get; set; }
        public string ProfileName { get; set; }

        public void Apply(AccountCreatedEvent @event)
        {
            Id = @event.Id;
            AccountName = @event.Command.AccountName;
            ProfileName = @event.Command.AccountName;
        }

        public void Apply(ProfileChangedEvent @event)
        {
            ProfileName = @event.Name;
        }
    }
}
