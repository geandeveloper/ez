using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace EzGym.Features.Payments.Gateways.GerenciaNet.Middlwares
{


    public class GerenciaNetAuthorizationMiddlware : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
