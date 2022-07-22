import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { DashboardComponent } from "./dashboard/dashboard.component";
import { UsersComponent } from "./users/users.component";


@NgModule({
  declarations: [
    DashboardComponent,
    UsersComponent
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule
  ],
  exports: [
    DashboardComponent,
    UsersComponent
  ]
})
export class AdminModule { }