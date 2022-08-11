import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from 'src/app/core/authentication/auth.guard';
import { ProfileComponent } from './accounts/profile/profile.component';

import { EzGymComponent } from './ezgym.component';
import { SearchAccountComponent } from './search/search-account/search-account.component';

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
      {
        path: 'search',
        component: SearchAccountComponent,
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