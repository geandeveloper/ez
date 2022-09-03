import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CloseWindowComponent } from './shared/components/close-window/close-window.component';
import { Error404Component } from './shared/error-pages/404/error-404.component';
import { Error500Component } from './shared/error-pages/500/error-500.component';

const routes: Routes = [
  {
    path: '404',
    component: Error404Component
  },
  {
    path: 'close',
    component: CloseWindowComponent
  },
  {
    path: '500',
    component: Error500Component
  },
  {
    path: 'ezpayment',
    loadChildren: () => import("./ezpayment/ezpayment.module").then(m => m.EzPaymentModule)

  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
