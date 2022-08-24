export interface GymModel {
    id: string
}

export enum PaymentTypeEnum {
    Pix = 1
}

export interface GymPlanModel {
    id: string,
    accountId: string,
    name: string,
    days: number,
    price: number,
    active: boolean
}