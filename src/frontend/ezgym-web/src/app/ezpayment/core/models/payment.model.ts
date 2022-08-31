
export interface PaymentModel {
    id: string,
    description: string,
    cardInfo: CreditCardInfo,
    amount: number
    status: PaymentStatusEnum
}

export enum PaymentStatusEnum {
    pending = 1,
    approved = 2
}

interface CreditCardInfo {
    integrationId: string,
    clientSecretKey: string
}

