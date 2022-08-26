using EzCommon.Infra.Bus;
using EzCommon.Infra.Repository;
using EzGym.Infra.Storage;

namespace EzGym.Infra.Repository
{
    public class GymRepository : BaseRepository<IGymEventStore>, IGymRepository
    {
        public GymRepository(IGymEventStore storage, IBus bus) : base(storage, bus)
        {
        }
    }
}
