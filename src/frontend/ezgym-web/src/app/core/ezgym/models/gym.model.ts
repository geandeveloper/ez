export interface GymModel {
    id: string
    accountId: string
}

export enum PaymentTypeEnum {
    Pix = 1,
    CreditCard = 2
}

export interface GymPlanModel {
    id: string,
    accountId: string,
    name: string,
    days: number,
    amount: number,
    active: boolean
}