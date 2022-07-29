import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { AccountsComponent } from './accounts.component';
import { CreateAccountComponent } from './create-account/create-account.component';
import { AccountRoutingModule } from './accounts-routing.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        AccountRoutingModule
    ],
    declarations: [
        AccountsComponent,
        CreateAccountComponent
    ],
    providers: [],
})
export class AccountsModule { }
