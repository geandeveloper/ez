using EzCommon.Integrations.Services.SearchAddress;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Refit;

namespace EzCommon
{
    public static class Api
    {
        public static WebApplication UseEzCommonApi(this WebApplication app)
        {
            app.MapGet("/integrations/services/search-address/{cep}", async (string cep) =>
            {
                var address = await RestService
                .For<ISearchAddressService>("https://viacep.com.br/")
                .SearchByCep(cep);

                return Results.Ok(new
                {
                    Cep = address.Cep,
                    Street = address.Logradouro,
                    Neighborhood = address.Bairro,
                    City = address.Localidade,
                    State = address.Uf,
                });
            });

            return app;
        }
    }
}
