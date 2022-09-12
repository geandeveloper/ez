import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateAccountComponent } from './create-account/create-account.component';
import { AccountManagementComponent } from './management/account-management.component';
import { ProfileComponent } from './profile/profile.component';

export const routes: Routes = [
    {
        path: '',
        component: ProfileComponent,
        outlet: 'content',
    },
    {
        path: 'create',
        component: CreateAccountComponent,
        outlet: 'content',
    },
    {
        path: 'management',
        component: AccountManagementComponent,
        outlet: 'content',
    },
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AccountRoutingModule { }