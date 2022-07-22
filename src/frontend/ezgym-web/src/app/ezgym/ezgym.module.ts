import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { EzGymComponent } from './ezgym.component';

import { AdminModule } from "./admin/admin.module";
import { EzGymRoutingModule } from "./ezgym-routing-module";


@NgModule({
  declarations: [
    EzGymComponent
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    EzGymRoutingModule,
    AdminModule
  ],
  exports: [
    EzGymComponent
  ]
})
export class EzGymModule { }