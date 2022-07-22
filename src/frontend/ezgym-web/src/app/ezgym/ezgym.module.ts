import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { EzGymComponent } from './ezgym.component';

import { AdminModule } from "./admin/admin.module";
import { RouterModule } from "@angular/router";


@NgModule({
  declarations: [
    EzGymComponent
  ],
  imports: [
    FormsModule,
    RouterModule,
    ReactiveFormsModule,
    AdminModule
  ],
  exports: [
    EzGymComponent
  ]
})
export class EzGymModule { }