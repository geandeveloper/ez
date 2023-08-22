using Marten.Events.Aggregation;
using Stripe;

namespace EzPayment.Projections
{
    public class SearchAccountsProjection : SingleStreamProjection<Account>
    {
    }
}
