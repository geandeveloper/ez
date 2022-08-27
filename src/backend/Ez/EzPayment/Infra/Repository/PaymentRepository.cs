using EzCommon.Infra.Bus;
using EzCommon.Infra.Repository;
using EzPayment.Infra.Storage;

namespace EzPayment.Infra.Repository
{
    public class PaymentRepository : BaseRepository<IPaymentStore>, IPaymentRepository
    {
        public PaymentRepository(IPaymentStore storage, IBus bus) : base(storage, bus)
        {
        }
    }
}
