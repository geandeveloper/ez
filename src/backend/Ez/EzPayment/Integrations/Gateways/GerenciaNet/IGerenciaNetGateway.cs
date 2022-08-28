using System.Threading.Tasks;
using EzPayment.Integrations.Gateways.GerenciaNet.GenerateQRCode;
using EzPayment.Integrations.Gateways.GerenciaNet.OAuthToken;
using EzPayment.Integrations.Gateways.GerenciaNet.RequestPayment;
using Refit;

namespace EzPayment.Integrations.Gateways.GerenciaNet
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
