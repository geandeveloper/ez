import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GymManagementComponent } from './gyms.component';

export const routes: Routes = [
  {
    path: 'management',
    component: GymManagementComponent,
    outlet: 'content',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GymsRoutingModule {}

