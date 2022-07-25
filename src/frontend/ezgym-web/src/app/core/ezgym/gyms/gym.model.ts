import { AddressModel } from '../address/address.model';

export interface GymModel {
    id: string,
    name: string,
    cnpj: string,
    addresses: AddressModel[]
}