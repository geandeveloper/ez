import { GymModule } from './gyms/gyms.module';
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { MatStepperModule } from '@angular/material/stepper';
import { MatFormFieldModule } from '@angular/material/form-field';

import { EzGymComponent } from './ezgym.component';

import { AdminModule } from "./admin/admin.module";
import { EzGymRoutingModule } from "./ezgym-routing.module";


@NgModule({
  declarations: [
    EzGymComponent
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    EzGymRoutingModule,
    AdminModule,
    MatStepperModule,
    MatFormFieldModule,
    GymModule
  ],
  exports: [
    EzGymComponent
  ]
})
export class EzGymModule { }