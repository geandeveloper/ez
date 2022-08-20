using Refit;
using System;
using System.Threading.Tasks;

namespace EzGym.Features.Payments.Gateways.GerenciaNet.RequestPayment
{
    public interface IGerenciaNetGateway
    {

        [Post("/v2/cob")]
        Task<RequestPaymentResponse> RequestPaymentAsync(RequestPaymentRequest request);
    }
}
