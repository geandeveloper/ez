import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from 'src/app/core/authentication/auth.guard';

import { EzGymComponent } from './ezgym.component';
import { SearchAccountComponent } from './search/search-account/search-account.component';

const routes: Routes = [
  {
    path: 'gyms/management',
    canActivate: [AuthGuard],
    component: EzGymComponent,
    loadChildren: () => import("./gyms/gyms.module").then(m => m.GymModule)
  },
  {
    path: 'accounts/search',
    component: EzGymComponent,
    children: [
      {
        path: '',
        component: SearchAccountComponent,
        outlet: 'content'
      }
    ]
  },
  {
    path: ':accountName',
    component: EzGymComponent,
    canActivate: [AuthGuard],
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