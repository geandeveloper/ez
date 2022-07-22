import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DashboardComponent } from './dashboard/dashboard.component';
import { UsersComponent } from './users/users.component';

const routes: Routes = [
  {
    path: 'admin',
    children: [
      { path: 'dashboard', component: DashboardComponent },
      { path: 'users', component: UsersComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }