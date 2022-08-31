import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';

import { EzGymComponent } from './ezgym.component';

import { EzGymRoutingModule } from "./ezgym-routing.module";
import { BrowserModule } from '@angular/platform-browser';
import { MatDialogModule } from "@angular/material/dialog";
import { ImageCropperModule } from "ngx-image-cropper";
import { SearchAccountComponent } from "./search/search-account/search-account.component";
import { WalletService } from "./core/services/wallet.service";
import { PaymentAccountService } from "../ezpayment/core/services/payment-account.service";


@NgModule({
  declarations: [
    EzGymComponent,
    SearchAccountComponent
  ],
  imports: [
    FormsModule,
    BrowserModule,
    MatDialogModule,
    ReactiveFormsModule,
    EzGymRoutingModule,
    MatAutocompleteModule,
    MatFormFieldModule,
    ImageCropperModule
  ],
  providers: [
    WalletService,
    PaymentAccountService
  ],
  exports: [
    EzGymComponent
  ]
})
export class EzGymModule { }