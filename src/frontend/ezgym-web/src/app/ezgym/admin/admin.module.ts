import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { AdminRoutingModule } from "./admin-routing-module";
import { DashboardComponent } from "./dashboard/dashboard.component";
import { UsersComponent } from "./users/users.component";


@NgModule({
  declarations: [
    DashboardComponent,
    UsersComponent
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    AdminRoutingModule
  ],
  exports: [
    DashboardComponent,
    UsersComponent
  ]
})
export class AdminModule { }