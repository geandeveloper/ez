using EzCommon.Infra.Bus;
using EzCommon.Infra.Storage;

namespace EzIdentity.Infra.Storage
{
    public class IdentityEventStore : StoreInLocal, IIdentityEventStore, IIdentityQueryStorage
    {
        public IdentityEventStore(IBus bus) : base(bus, "ezidentity")
        {
        }
    }
}
