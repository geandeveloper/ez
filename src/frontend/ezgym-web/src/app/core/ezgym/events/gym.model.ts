import { AddressModel } from '../models/address.model';

export interface GymCreatedEvent {
    id: string,
    fantasyName: string,
    cnpj: string,
    addresses: AddressModel[]
}