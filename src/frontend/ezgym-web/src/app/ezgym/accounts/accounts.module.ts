import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { CreateAccountComponent } from './create-account/create-account.component';
import { AccountRoutingModule } from './accounts-routing.module';
import { FollowerListComponent } from './follower-list/follower-list.component';
import { MatTabsModule } from '@angular/material/tabs';
import { ProfileComponent } from './profile/profile.component';
import { EditProfileComponent } from './profile/edit-profile/edit-profile.component';
import { ImageCropperModule } from 'ngx-image-cropper';
import { MatStepperModule } from '@angular/material/stepper';
import { ContentLoaderModule } from '@ngneat/content-loader';
import { AccountManagementComponent } from './accounts.component';
import { SharedComponentsModule } from 'src/app/shared/components/shared.module';
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
    ContentLoaderModule,
    SharedComponentsModule,
    GymModule,
  ],
  declarations: [
    CreateAccountComponent,
    FollowerListComponent,
    EditProfileComponent,
    ProfileComponent,
    AccountManagementComponent,
  ],
})
export class AccountsModule {}
