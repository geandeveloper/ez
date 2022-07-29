import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

import { GymsRoutingModule } from './gyms-routing.module';
import { CreateAccountComponent } from './create-account/create-account.component';
import { GymsComponent } from './gyms.component';
import { CommonModule } from '@angular/common';

@NgModule({
    imports: [
        GymsRoutingModule,
        CommonModule,
        FormsModule,
        ReactiveFormsModule
    ],
    declarations: [GymsComponent, CreateAccountComponent],
    providers: [],
})
export class GymModule { }
