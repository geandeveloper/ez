using EzCommon.CommandHandlers;
using EzCommon.Commands;
using EzCommon.Models;
using EzGym.Features.Payments.Gateways.GerenciaNet.RequestPayment;
using EzGym.Infra.Storage;
using Gerencianet.NETCore.SDK;
using Refit;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EzGym.Features.Payments.Pix
{
    public record CreatePixCommand(decimal Value, Guid PayerAccountId, Guid ReceiverAccountId) : ICommand;

    public record CreatePixCommandHandler : ICommandHandler<CreatePixCommand>
    {

        public async Task<EventStream> Handle(CreatePixCommand request, CancellationToken cancellationToken)
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ0eXBlIjoiYWNjZXNzX3Rva2VuIiwiY2xpZW50SWQiOiJDbGllbnRfSWRfYWVmZWM3ZDU0MzgzMzI0NTYwY2IxNTBmNTFlYjVhNmNiZjZkZmE3OCIsImFjY291bnQiOjQwODI4NywiYWNjb3VudF9jb2RlIjoiZWJiMjdhMzgxODgyMGI1MTRlZmU3OWRmZDg3ODk0MDQiLCJzY29wZXMiOlsiY29iLnJlYWQiLCJjb2Iud3JpdGUiLCJnbi5iYWxhbmNlLnJlYWQiLCJnbi5waXguZXZwLnJlYWQiLCJnbi5waXguZXZwLndyaXRlIiwiZ24ucmVwb3J0cy5yZWFkIiwiZ24ucmVwb3J0cy53cml0ZSIsImduLnNldHRpbmdzLnJlYWQiLCJnbi5zZXR0aW5ncy53cml0ZSIsInBheWxvYWRsb2NhdGlvbi5yZWFkIiwicGF5bG9hZGxvY2F0aW9uLndyaXRlIiwicGl4LnJlYWQiLCJwaXguc2VuZCIsInBpeC53cml0ZSIsIndlYmhvb2sucmVhZCIsIndlYmhvb2sud3JpdGUiXSwiZXhwaXJlc0luIjozNjAwLCJjb25maWd1cmF0aW9uIjp7Ing1dCNTMjU2IjoiVTJPNkpqbVF0QzNwRjRPb0VyUlVKZlcwU0NUa283S1Q2ZnkvVC9wZXliST0ifSwiaWF0IjoxNjYwOTczMzgxLCJleHAiOjE2NjA5NzY5ODF9.FZznRhcXcl6W1cys8syinC0BnpaYa17y5_rF69eMOAc";

            var httpClient = new HttpClient(CreateHandler())
            {
                BaseAddress = new Uri(@"https://api-pix.gerencianet.com.br"),
            };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var gerenciaNet = RestService.For<IGerenciaNetGateway>(httpClient);

            var payload = new RequestPaymentRequest
            {
                Calendario = new Calendario
                {
                    Expiracao = 3600
                },
                Chave = "d7d6bd52-0c8a-4b3e-a16c-1c8d267dd68d",
                SolicitacaoPagador = "teste",
                Valor = new Valor
                {
                    Original = "1.00"
                }

            };

            var @json = JsonSerializer.Serialize(payload);


            var test = await gerenciaNet.RequestPaymentAsync(payload);
            return null;
            //var payment = new Payment(request);
            //return await _eventStore.SaveAggregateAsync(payment);
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
    }
}
