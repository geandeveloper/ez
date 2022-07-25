using EzCommon.Integrations.Services.SearchAddress;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Refit;

namespace EzCommon
{
    public static class EzCommonApi
    {
        public static IApplicationBuilder UseEzCommonApi(this WebApplication app)
        {
            app.MapGet("/integrations/services/search-address/{cep}", async (string cep) =>
            {
                var address = await RestService
                .For<ISearchAddressService>("https://viacep.com.br/")
                .SearchByCep(cep);

                return Results.Ok(address);
            });

            return app;
        }
    }
}
