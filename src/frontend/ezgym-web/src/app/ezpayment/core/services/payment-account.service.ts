import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { PaymentAccountStatusChanged } from "../events/payment-account.evets";

@Injectable()
export class PaymentAccountService {
    constructor(private http: HttpClient) { }

    verifyAccountPayment(command: { paymentAccountId: string }): Observable<PaymentAccountStatusChanged> {
        return this.http.post<PaymentAccountStatusChanged>(`/payment-accounts/${command.paymentAccountId}/verify`, {})
    }
}