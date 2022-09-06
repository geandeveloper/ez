import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { PaymentAccountChangedEvent } from "../events/wallet.events";
import { WalletModel, WalletReceiptModel, WalletStatementModel } from "../models/wallet.model";

@Injectable()
export class WalletService {
    constructor(private http: HttpClient) { }

    setupPaymentAccount(command: { walletId: string, refreshUrl: string, returnUrl: string }): Observable<PaymentAccountChangedEvent> {
        return this.http.put<PaymentAccountChangedEvent>(`wallets/${command.walletId}/setup-payment-account`, command)
    }

    getWallet(id: string): Observable<WalletModel> {
        return this.http.get<WalletModel>(`wallets/${id}`)
    }

    getReceipts(walletId: string): Observable<WalletReceiptModel[]> {
        return this.http.get<WalletReceiptModel[]>(`wallets/${walletId}/receipts`)
    }

    getStatement(walletId: string): Observable<WalletStatementModel> {
        return this.http.get<WalletStatementModel>(`wallets/${walletId}/statement`)
    }

}