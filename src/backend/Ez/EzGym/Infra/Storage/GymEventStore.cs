using EzCommon.Infra.Bus;
using EzCommon.Infra.Storage;

namespace EzGym.Infra.Storage
{
    public class GymEventStore : StoreInLocal, IGymEventStore, IGymQueryStore
    {
        public GymEventStore(IBus bus) : base(bus, "ezgym")
        {
        }
    }
}
