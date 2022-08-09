import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { AccountsComponent } from './accounts.component';
import { CreateAccountComponent } from './create-account/create-account.component';
import { AccountRoutingModule } from './accounts-routing.module';
import { FollowerListComponent } from './follower-list/follower-list.component';
import { MatTabsModule } from '@angular/material/tabs';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        AccountRoutingModule,
        MatTabsModule
    ],
    declarations: [
        AccountsComponent,
        CreateAccountComponent,
        FollowerListComponent
    ],
    providers: [],
})
export class AccountsModule { }
