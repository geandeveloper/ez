using EzCommon.Integrations.Services.SearchByCep;
using Refit;
using System.Threading.Tasks;

namespace EzCommon.Integrations.Services.SearchAddress
{
    public interface ISearchAddressService
    {
        [Get("/ws/{cep}/json")]
        Task<SearchAddressByCepResponse> SearchByCep(string cep);
        
    }
}
