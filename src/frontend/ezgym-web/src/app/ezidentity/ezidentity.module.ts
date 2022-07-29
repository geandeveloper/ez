import { NgModule } from "@angular/core";
import { CommonModule } from '@angular/common';

import { LoginComponent } from './login/login.component';
import { EzIdentityRoutingModule } from "./ezidentity-routing.module";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { CreateUserComponent } from "./create-user/create-user.component";

@NgModule({
  declarations: [LoginComponent, CreateUserComponent],
  imports: [
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    EzIdentityRoutingModule
  ],
  exports: [LoginComponent, CreateUserComponent]
})
export class EzIdentityModule { }