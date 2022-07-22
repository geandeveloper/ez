import { NgModule } from "@angular/core";

import { LoginComponent } from './login/login.component';
import { AuthenticationRoutingModule } from "./ezidentity-routing.module";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { CreateUserComponent } from "./create-user/create-user.component";

@NgModule({
  declarations: [LoginComponent, CreateUserComponent],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    AuthenticationRoutingModule
  ],
  exports: [LoginComponent, CreateUserComponent]
})
export class EzIdentityModule { }