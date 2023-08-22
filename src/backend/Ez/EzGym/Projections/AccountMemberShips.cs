using EzGym.Events.Gym;
using EzGym.Gyms;
using System.Linq;
using Marten.Events.Aggregation;
using EzGym.Accounts;
using System;

namespace EzGym.Projections
{
    public class AccountMemberShip
    {
        public string Id { get; set; }
        public string AccountId { get; set; }
        public string GymAccountId { get; set; }
        public string GymProfileName { get; set; }
        public string GymAccountName { get; set; }
        public string GymAvatarUrl { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public DateTime EndDateTime => PaymentDateTime.AddDays(Days);
        public int Days { get; set; }
        public int MissingDays => (EndDateTime - DateTime.UtcNow).Days;
    }

    public class AccountMemberShipProjection : SingleStreamProjection<AccountMemberShip>
    {
        public AccountMemberShipProjection()
        {
            ProjectEvent<GymMemberShipPaidEvent>((session, state, @event) => {

                var memberShip = session
                .Query<GymMemberShip>()
                .First(m => m.Id == @event.Id);

                var account = session
                .Query<Account>()
                .First(c => c.Id == memberShip.ReceiverAccountId);

                state.Id = @event.Id;
                state.AccountId = memberShip.PayerAccountId;
                state.GymAccountId = memberShip.ReceiverAccountId;
                state.GymAccountName = account.AccountName;
                state.GymProfileName = account.Profile?.Name;
                state.GymAvatarUrl = account.AvatarUrl;
                state.PaymentDateTime = @event.PaymentDateTime;
                state.Days = memberShip.Days;
            });
        }
    }




























}
