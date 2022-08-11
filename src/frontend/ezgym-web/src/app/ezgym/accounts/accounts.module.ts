import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { AccountsComponent } from './accounts.component';
import { CreateAccountComponent } from './create-account/create-account.component';
import { AccountRoutingModule } from './accounts-routing.module';
import { FollowerListComponent } from './follower-list/follower-list.component';
import { MatTabsModule } from '@angular/material/tabs';
import { RegisterMembershipComponent } from './gyms/register-membership/register-membership.component';
import { GymProfileComponent } from './gyms/gym-profile/gym-profile.component';
import { ProfileComponent } from './profile/profile.component';
import { EditProfileComponent } from './profile/edit-profile/edit-profile.component';
import { ImageCropperModule } from 'ngx-image-cropper';
import { MatStepperModule } from '@angular/material/stepper'
import { MatFormFieldModule } from '@angular/material/form-field'

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        AccountRoutingModule,
        MatTabsModule,
        ImageCropperModule,
        MatStepperModule,
        MatFormFieldModule
    ],
    declarations: [
        AccountsComponent,
        CreateAccountComponent,
        FollowerListComponent,
        RegisterMembershipComponent,
        EditProfileComponent,
        ProfileComponent,
        GymProfileComponent,
    ],
    providers: [],
})
export class AccountsModule { }
