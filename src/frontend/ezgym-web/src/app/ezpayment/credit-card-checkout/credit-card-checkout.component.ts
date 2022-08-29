import { Component, OnInit, ViewChild } from '@angular/core';

import { StripeService, StripePaymentElementComponent } from 'ngx-stripe';
import {
    StripeCardElementOptions,
    StripeElementsOptions
} from '@stripe/stripe-js';
import { FormBuilder, Validators } from '@angular/forms';


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

    paymentForm = this.fb.group({
        name: ['John doe', [Validators.required]],
        email: ['support@ngx-stripe.dev', [Validators.required]],
        amount: [2500, [Validators.required, Validators.pattern(/d+/)]]
    });

    constructor(
        private fb: FormBuilder,
        private stripeService: StripeService
    ) {

    }

    ngOnInit() {
        this.elementsOptions.appearance = {
            theme: 'stripe',
            variables: {
                fontFamily: 'poppins',
                colorTextPlaceholder: '#ccc',
                colorTextSecondary: '#000'
            } 

        }
        this.elementsOptions.clientSecret = "pi_3Lbh4rGAlQ5T9hwX0sP7dFj8_secret_3Fbpo7HGlYofaKpXAhKx616nb"
    }

    pay() {
        this.stripeService.confirmPayment({
            elements: this.paymentElement.elements,
            redirect: 'if_required',
            confirmParams: {
                return_url: "https://localhost:4200"
            }
        }).subscribe(teste => {
            debugger
        })
    }
}