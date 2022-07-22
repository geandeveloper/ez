import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from 'src/app/core/authentication/auth.guard';

import { EzGymComponent } from './ezgym.component';

const routes: Routes = [
  {
    path: '',
    component: EzGymComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'admin', loadChildren: () => import("./admin/admin-routing-module").then(m => m.AdminRoutingModule)
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class EzGymRoutingModule { }