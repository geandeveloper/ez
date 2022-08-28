using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using EzPayment.Integrations.Gateways.GerenciaNet;
using EzPayment.Integrations.Gateways.GerenciaNet.OAuthToken;
using EzPayment.Integrations.Gateways.StripePayments;
using Microsoft.Extensions.Options;
using Refit;
using Stripe;

namespace EzPayment.Integrations.Gateways
{
    public class GatewayFactory
    {
        public OAuthTokenResponse Authorization { get; private set; }

        private readonly IStripePaymentGateway _stripePaymentGateway;

        public GatewayFactory(IOptions<EzPaymentSettings> settings, IStripePaymentGateway stripePaymentGateway)
        {
            StripeConfiguration.ApiKey = settings.Value.StripePayments.ApiSecretKey;
            _stripePaymentGateway = stripePaymentGateway;
        }

        public T UseGerenciaNet<T>(Func<IGerenciaNetGateway, T> useGateway)
        {
            var httpClient = new HttpClient(CreateHandler())
            {
                BaseAddress = new Uri(@"https://api-pix.gerencianet.com.br"),
            };

            var authorizationHeader = Base64Encode($"Client_Id_aefec7d54383324560cb150f51eb5a6cbf6dfa78:Client_Secret_05e2285dc41662b3c6385f48fadd1fcc0beeae74");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorizationHeader);

            var gerenciaNetGateway = RestService.For<IGerenciaNetGateway>(httpClient);

            Authorization ??= gerenciaNetGateway.GenerateOAuthTokenAsync(new OAuthTokenRequest()).ConfigureAwait(false).GetAwaiter().GetResult();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Authorization.Access_token);

            return useGateway(gerenciaNetGateway);
        }

        public T UseStripePayment<T>(Func<IStripePaymentGateway, T> useGateway)
        {
            return useGateway(_stripePaymentGateway);
        }

        private static HttpClientHandler CreateHandler()
        {
            var certificate = new X509Certificate2("C:/secrets/producao-408287-ezgym-prod.p12", string.Empty);

            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual
            };

            handler.ClientCertificates.Add(certificate);
            return handler;
        }
        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

    }
}
