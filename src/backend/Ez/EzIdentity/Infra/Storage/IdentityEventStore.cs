using EzCommon.Infra.Bus;
using EzCommon.Infra.Storage;

namespace EzIdentity.Infra.Storage
{
    public class IdentityEventStore : MongoStorage, IIdentityEventStore, IIdentityQueryStore
    {
        public IdentityEventStore(IBus bus) : base(bus, "ezidentity")
        {
        }
    }
}
