import { AddressModel } from '../models/address.model';

export interface GymCreatedEvent {
    id: string,
    fantasyName: string,
    cnpj: string,
    addresses: AddressModel[]
}

export interface PlanCreatedEvent {
    id: string,
    command: {
        accountId: string,
        gymId: string,
        name: string
        days: string
        price: string
        active: string
    }
}

export interface GymMemberShipRegisteredEvent {
    id: string,
    gymMemberShip: {
        paymentId: string
    }
}