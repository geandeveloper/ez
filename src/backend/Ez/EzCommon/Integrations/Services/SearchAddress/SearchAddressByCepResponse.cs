namespace EzCommon.Integrations.Services.SearchByCep
{
    public record SearchAddressByCepResponse(
        string Cep,
        string Logradouro,
        string Complemento,
        string Bairro,
        string Localidade,
        string Uf,
        string Ibge,
        string Gia,
        string Ddd,
        string Siafi
        );
}
