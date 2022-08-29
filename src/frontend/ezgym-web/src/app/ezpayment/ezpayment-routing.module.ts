import { NgModule } from "@angular/core";
import { CreditCardCheckoutComponent } from "./credit-card-checkout/credit-card-checkout.component";

import { RouterModule, Routes } from "@angular/router";
import { EzPaymentComponent } from "./ezpayment.component";
import { AuthGuard } from "../core/authentication/auth.guard";

const routes: Routes = [
  {
    path: ':id',
    component: EzPaymentComponent,
    // canActivate: [AuthGuard],
    children: [
      { path: 'credit-card', component: CreditCardCheckoutComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EzPaymentRoutingModule { }