import { PaymentAccountStatusEnum } from "src/app/ezpayment/core/models/payment-account.model"

export interface PaymentAccountChangedEvent {
    walletId: string,
    paymentAccountId: string,
    onBoardingLink: string,
    paymentAccountStatus: PaymentAccountStatusEnum
}