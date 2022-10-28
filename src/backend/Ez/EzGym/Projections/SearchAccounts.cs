using EzGym.Accounts;
using EzGym.Events.Accounts;
using Marten.Events.Aggregation;

namespace EzGym.Projections
{
    public class SearchAccounts
    {
        public string Id { get; set; }
        public string AccountName { get; set; }
        public string ProfileName { get; set; }
        public AccountTypeEnum AccountType { get; set; }
        public string AvatarUrl { get; set; }

    }

    public class SearchAccountsProjection : SingleStreamAggregation<SearchAccounts>
    {
        public SearchAccountsProjection()
        {
            ProjectEvent<AccountCreatedEvent>((state, @event) =>
            {
                state.Id = @event.Id;
                state.AccountName = @event.Command.AccountName;
                state.ProfileName = @event.Command.AccountName;
                state.AccountType = @event.Command.AccountType;
            });

            ProjectEvent<ProfileChangedEvent>((state, @event) =>
            {
                state.ProfileName = @event.Name;
            });

            ProjectEvent<AvatarImageAccountChangedEvent>((state, @event) =>
            {
                state.AvatarUrl = @event.AvatarUrl;
            });

        }

    }
}
