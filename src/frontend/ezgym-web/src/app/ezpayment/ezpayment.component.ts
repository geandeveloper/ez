import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { switchMap, tap } from 'rxjs';
import { PaymentStatusEnum } from './core/models/payment.model';
import { EzPaymentStore } from './ezpayment.store';

@Component({
    selector: 'ezpayment',
    templateUrl: 'ezpayment.component.html',
    styleUrls: ['./ezpayment.component.scss'],
})

export class EzPaymentComponent implements OnInit {
    constructor(
        private activeRoute: ActivatedRoute,
        private router: Router,
        private ezPaymentStore: EzPaymentStore
    ) { }

    ngOnInit() {
        this.activeRoute.params
            .pipe(
                switchMap(params => this.ezPaymentStore.loadPayment(params["id"])),
                tap(payment => {
                    if (payment.status == PaymentStatusEnum.approved)
                        this.router.navigate([`/ezpayment/${payment.id}/success-payment`])
                    else
                        this.router.navigate([`/ezpayment/${payment.id}/credit-card`])
                })
            ).subscribe()
    }
}