import { PaymentAccountStatusEnum } from "src/app/ezpayment/core/models/payment-account.model"

export interface WalletModel {
    id: string,
    balance: number
    pix: Pix,
    paymentAccount?: PaymentAccountModel
}

export interface PaymentAccountModel {
    id: string,
    paymentAccountStatus: PaymentAccountStatusEnum
}

export interface Pix {
    type: PixTypeEnum
    value: string
}

export enum PixTypeEnum {
    PhoneNumber = 1,
    Email = 2,
    Random = 3
}