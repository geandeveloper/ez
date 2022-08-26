using EzCommon.Infra.Bus;
using EzCommon.Infra.Repository;
using EzIdentity.Infra.Storage;

namespace EzIdentity.Infra.Repository
{
    public class IdentityRepository : BaseRepository<IIdentityEventStore>, IIdentityRepository
    {
        public IdentityRepository(IIdentityEventStore storage, IBus bus) : base(storage, bus)
        {
        }
    }
}
