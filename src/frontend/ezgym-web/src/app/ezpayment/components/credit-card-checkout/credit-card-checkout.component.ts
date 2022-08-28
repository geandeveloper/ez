import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Appearance, StripeElement, StripeElements } from '@stripe/stripe-js';

import { loadStripe } from '@stripe/stripe-js/pure';

import { StripeService, StripePaymentElementComponent } from 'ngx-stripe';
import {
    StripeElementsOptions,
    PaymentIntent
} from '@stripe/stripe-js';


@Component({
    selector: 'credit-card-checkout',
    templateUrl: 'credit-card-checkout.component.html',
    styleUrls: ['credit-card-checkout.component.scss']
})
export class CreditCardCheckoutComponent implements OnInit {

    @ViewChild(StripePaymentElementComponent)
    paymentElement!: StripePaymentElementComponent;


    elementsOptions: StripeElementsOptions = {
        locale: 'pt-BR'
    };


    constructor(private stripeService: StripeService) {

    }

    ngOnInit() {
        this.elementsOptions.clientSecret = "pi_3LbN7uGAlQ5T9hwX0V2xNvK6_secret_8FmgkcnSkD5dzhKbGUIjldJ7w"
    }

    pay() {
        this.stripeService.confirmPayment({
            elements: this.paymentElement.elements,
            confirmParams: {
                return_url: "https://localhost:4200"
            }
        }).subscribe(teste => {
            debugger
        })
    }
}