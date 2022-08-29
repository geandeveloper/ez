import { Injectable } from "@angular/core";
import { tap } from "rxjs";
import { Store } from "../core/state/store";
import { PaymentModel } from "./core/models/payment.model";
import { PaymentService } from "./core/services/payment.service";

export interface EzPaymentState {
    payment: PaymentModel,
    redirecUrl: string
}

@Injectable()
export class EzPaymentStore extends Store<EzPaymentState> {

    constructor(
        private paymentService: PaymentService
    ) {
        super({
            payment: {} as PaymentModel,
            redirecUrl: 'http://localhost:4200'
        })
    }

    loadPayment(id: string) {
        return this.paymentService.getPayment(id)
            .pipe(
                tap(payment => {
                    this.setState(state => ({
                        ...state,
                        payment
                    }))
                })
            )
    }

}