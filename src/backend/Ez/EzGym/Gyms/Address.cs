namespace EzGym.Gyms
{
    public record Address(
        string Cep,
        string Street,
        string Number,
        string City,
        string State,
        string ExtraInformation);
}
