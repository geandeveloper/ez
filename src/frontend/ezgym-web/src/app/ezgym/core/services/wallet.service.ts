import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { PaymentAccountChangedEvent } from "../events/wallet.events";

@Injectable()
export class WalletService {
    constructor(private http: HttpClient) { }

    setupPaymentAccount(command: { walletId: string, refreshUrl: string, returnUrl: string }): Observable<PaymentAccountChangedEvent> {
        return this.http.put<PaymentAccountChangedEvent>(`wallets/${command.walletId}/setup-payment-account`, command)
    }

}