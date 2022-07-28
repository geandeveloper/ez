import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateGymComponent } from './create-gym/create-gym.component';

export const routes: Routes = [
    {
        path: 'create',
        component: CreateGymComponent,
        outlet: 'content',
        data: { animation: 'isRight' }
    }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class GymsRoutingModule { }