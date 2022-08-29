import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { PaymentModel } from "../models/payment.model";

@Injectable()
export class PaymentService {
    constructor(private http: HttpClient) { }

    getPayment(id: string) {
        return this.http.get<PaymentModel>(`/payments/${id}/`)
    }
}