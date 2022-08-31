import { NgModule } from "@angular/core";
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { CreditCardCheckoutComponent } from "./credit-card-checkout/credit-card-checkout.component";

import { NgxStripeModule } from 'ngx-stripe';
import { EzPaymentRoutingModule } from "./ezpayment-routing.module";
import { EzPaymentComponent } from "./ezpayment.component";
import { PaymentService } from "./core/services/payment.service";
import { SuccessPaymentComponent } from "./success-payment/success-payment/success-payment.component";
import { EzPaymentStore } from "./ezpayment.store";
import { PaymentAccountService } from "./core/services/payment-account.service";

@NgModule({
  declarations: [
    CreditCardCheckoutComponent,
    EzPaymentComponent,
    SuccessPaymentComponent
  ],
  exports: [
    CreditCardCheckoutComponent
  ],
  providers: [
    PaymentService,
    EzPaymentStore,
    PaymentAccountService
  ],
  imports: [
    EzPaymentRoutingModule,
    NgxStripeModule.forRoot('pk_test_51LQOtvGAlQ5T9hwXwEiLUeWzvgOu94n74ZCrwOd4Jb4K534alcccN4QvHrieOPgR4pQBV7OuvLsOLu0YwjyqQNxs00fOlyghlo'),
    FormsModule,
    CommonModule,
    ReactiveFormsModule
  ]
})
export class EzPaymentModule { }