import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from 'src/app/core/authentication/auth.guard';

import { EzGymComponent } from './ezgym.component';
import { ProfileComponent } from './profile/profile.component';

const routes: Routes = [
  {
    path: ':accountName',
    component: EzGymComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        component: ProfileComponent,
        outlet: 'content'
      },
    ]
  },
  {
    path: 'accounts',
    canActivate: [AuthGuard],
    component: EzGymComponent,
    loadChildren: () => import("./accounts/accounts.module").then(m => m.AccountsModule)
  },
  {
    path: '',
    pathMatch: 'full',
    redirectTo: '/ezidentity/login'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class EzGymRoutingModule { }