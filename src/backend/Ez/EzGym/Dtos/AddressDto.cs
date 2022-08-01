namespace EzGym.Dtos
{
    public record AddressDto(
        string Id,
        string Cep,
        string Street,
        string Number,
        string City,
        string State,
        string ExtraInformation);
}
