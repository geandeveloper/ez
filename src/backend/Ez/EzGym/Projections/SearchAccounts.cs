using EzGym.Accounts.Events;
using Marten.Events.Aggregation;

namespace EzGym.Projections
{
    public class SearchAccounts
    {
        public string Id { get; set; }
        public string AccountName { get; set; }
        public string ProfileName { get; set; }

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
            });

            ProjectEvent<ProfileChangedEvent>((state, @event) =>
            {
                state.ProfileName = @event.Name;
            });

        }

    }
}
