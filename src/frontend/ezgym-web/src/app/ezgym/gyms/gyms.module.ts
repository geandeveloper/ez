import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

import { GymsRoutingModule } from './gyms-routing.module';
import { CreateGymComponent } from './create-gym/create-gym.component';
import { GymsComponent } from './gyms.component';
import { CommonModule } from '@angular/common';

@NgModule({
    imports: [
        GymsRoutingModule,
        CommonModule,
        FormsModule,
        ReactiveFormsModule
    ],
    declarations: [GymsComponent, CreateGymComponent],
    providers: [],
})
export class GymModule { }
