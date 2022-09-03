import { Component, OnInit } from '@angular/core';
import { Store } from 'src/app/core/state/store';
import { EzPaymentStore } from '../../ezpayment.store';

@Component({
    selector: 'error-500',
    templateUrl: './success-payment.component.html',
    styleUrls: ['success-payment.component.scss']

})

export class SuccessPaymentComponent extends Store<string> implements OnInit {

    constructor(private ezPaymentStore: EzPaymentStore) {
        super("")

        this.ezPaymentStore.store$.subscribe(state => {
            this.setState(() => (state.payment.redirectUrl))
        })
    }

    ngOnInit() { }
}