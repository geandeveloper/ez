export interface AddressModel {
    cep: string,
    street: string,
    neighborhood: string,
    number: string,
    city: string,
    state: string,
    extraInformation?: string
}