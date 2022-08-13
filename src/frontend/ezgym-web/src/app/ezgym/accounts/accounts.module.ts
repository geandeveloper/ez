import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { AccountsComponent } from './accounts.component';
import { CreateAccountComponent } from './create-account/create-account.component';
import { AccountRoutingModule } from './accounts-routing.module';
import { FollowerListComponent } from './follower-list/follower-list.component';
import { MatTabsModule } from '@angular/material/tabs';
import { ProfileComponent } from './profile/profile.component';
import { EditProfileComponent } from './profile/edit-profile/edit-profile.component';
import { ImageCropperModule } from 'ngx-image-cropper';
import { MatStepperModule } from '@angular/material/stepper'
import { GymModule } from '../gyms/gyms.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        AccountRoutingModule,
        MatTabsModule,
        ImageCropperModule,
        MatStepperModule,
        GymModule
    ],
    declarations: [
        AccountsComponent,
        CreateAccountComponent,
        FollowerListComponent,
        EditProfileComponent,
        ProfileComponent,
    ],
    providers: [],
})
export class AccountsModule { }
