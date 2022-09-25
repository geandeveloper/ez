import { Component, OnInit, ViewChild } from '@angular/core';

import { StripeService, StripePaymentElementComponent } from 'ngx-stripe';
import { StripeElementsOptions } from '@stripe/stripe-js';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize, tap } from 'rxjs';
import { PaymentModel } from '../core/models/payment.model';
import { Store } from 'src/app/core/state/store';
import { PreLoaderStore } from 'src/app/shared/components/pre-loader/pre-loader.store';
import { EzPaymentStore } from '../ezpayment.store';

interface ComponentState {
  ui?: {
    stripeElementOptions: StripeElementsOptions;
  };
  payment?: PaymentModel;
}

@Component({
  selector: 'credit-card-checkout',
  templateUrl: 'credit-card-checkout.component.html',
  styleUrls: ['credit-card-checkout.component.scss'],
})
export class CreditCardCheckoutComponent
  extends Store<ComponentState>
  implements OnInit
{
  @ViewChild(StripePaymentElementComponent)
  paymentElement!: StripePaymentElementComponent;

  paymentForm = this.fb.group({
    name: ['', [Validators.required]],
    email: ['', [Validators.required]],
    amount: [2500, [Validators.required, Validators.pattern(/d+/)]],
  });

  constructor(
    private fb: FormBuilder,
    private stripeService: StripeService,
    private router: Router,
    private ezPaymentStore: EzPaymentStore,
    private preloader: PreLoaderStore
  ) {
    super({
      ui: {
        stripeElementOptions: {
          locale: 'pt-BR',
          appearance: {
            theme: 'stripe',
            variables: {
              fontFamily: 'poppins',
              colorTextPlaceholder: '#ccc',
              colorTextSecondary: '#000',
            },
          },
        },
      },
    });
  }

  ngOnInit() {
    this.ezPaymentStore.store$.subscribe((paymentState) => {
      this.setState((state) => ({
        ...state,
        payment: paymentState.payment,
        ui: {
          stripeElementOptions: state.ui?.stripeElementOptions ?? {},
        },
      }));
    });
  }

  confirmPayment() {
    this.preloader.show();
    this.stripeService
      .confirmPayment({
        elements: this.paymentElement.elements,
        redirect: 'if_required',
        confirmParams: {
          return_url: this.state.payment?.redirectUrl,
        },
      })
      .pipe(
        tap((payment) => {
          if (payment.paymentIntent?.status == 'succeeded')
            this.router.navigate([
              `/ezpayment/${this.state.payment?.id}/success-payment`,
            ]);
          else alert('error during payment, try again');
        }),
        finalize(() => {
          this.preloader.close();
        })
      )
      .subscribe();
  }
}

