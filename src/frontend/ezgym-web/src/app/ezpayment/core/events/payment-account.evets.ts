import { PaymentAccountStatusEnum } from "../models/payment-account.model";

export interface PaymentAccountStatusChanged {
    paymentAccountId: string,
    paymentAccountStatus: PaymentAccountStatusEnum
}