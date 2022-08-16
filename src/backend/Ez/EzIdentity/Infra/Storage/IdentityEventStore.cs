using EzCommon.Infra.Bus;
using EzCommon.Infra.Storage;
using EzIdentity.Models;
using Marten;

namespace EzIdentity.Infra.Storage
{
    public class IdentityEventStore : MartenStorage, IIdentityEventStore, IIdentityQueryStore
    {
        public IdentityEventStore(IBus bus, IDocumentSession session) : base(bus, session)
        {
        }
    }
}
