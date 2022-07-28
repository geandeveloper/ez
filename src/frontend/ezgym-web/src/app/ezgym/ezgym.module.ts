import { GymModule } from './gyms/gyms.module';
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';

import { EzGymComponent } from './ezgym.component';

import { EzGymRoutingModule } from "./ezgym-routing.module";


@NgModule({
  declarations: [
    EzGymComponent
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    EzGymRoutingModule,
    MatAutocompleteModule,
    MatFormFieldModule,
    GymModule
  ],
  exports: [
    EzGymComponent
  ]
})
export class EzGymModule { }