import { PaymentAccountStatusEnum } from "src/app/ezpayment/core/models/payment-account.model"

export interface WalletModel {
    id: string,
    pix: Pix,
    paymentAccount?: PaymentAccountModel,
    statement: WalletStatementModel
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

export interface WalletReceiptModel {
    id: string,
    balance: number
    pix: Pix,
    paymentAccount?: PaymentAccountModel
}

export interface WalletStatementModel {
    totalApproved: number,
    totalPending: number,
    balance: number
}