using System.Threading.Tasks;
using EzGym.Payments.Gateways.GerenciaNet.GenerateQRCode;
using EzGym.Payments.Gateways.GerenciaNet.OAuthToken;
using EzGym.Payments.Gateways.GerenciaNet.RequestPayment;
using Refit;

namespace EzGym.Payments.Gateways.GerenciaNet
{
    public interface IGerenciaNetGateway
    {

        [Post("/oauth/token")]
        Task<OAuthTokenResponse> GenerateOAuthTokenAsync(OAuthTokenRequest request);


        [Post("/v2/cob")]
        Task<RequestPaymentResponse> RequestPaymentAsync(RequestPaymentRequest request);


        [Get("/v2/loc/{locationId}/qrcode")]
        Task<GenerateQRCodeResponse> GenerateQRCodeAsync(int locationId);
    }
}
