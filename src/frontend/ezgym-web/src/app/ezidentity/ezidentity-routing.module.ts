import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateUserComponent } from './create-user/create-user.component';

import { LoginComponent } from './login/login.component';

const routes: Routes = [
  {
    path: 'ezidentity',
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'create', component: CreateUserComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AuthenticationRoutingModule { }