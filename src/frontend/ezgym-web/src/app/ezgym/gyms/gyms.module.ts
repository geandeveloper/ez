import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MatStepperModule } from '@angular/material/stepper';
import { MatTabsModule } from '@angular/material/tabs';
import { ImageCropperModule } from 'ngx-image-cropper';
import { GymPlansComponent } from './gym-plans/gym-plans.component';

import { GymProfileComponent } from './gym-profile/gym-profile.component';
import { GymWalletComponent } from './gym-wallet/gym-wallet.component';
import { GymsRoutingModule } from './gyms-routing.module';
import { GymManagementComponent } from './gyms.component';
import { RegisterMembershipComponent } from './register-membership/register-membership.component';

@NgModule({
  imports: [
    CommonModule,
    ImageCropperModule,
    MatTabsModule,
    MatStepperModule,
    FormsModule,
    ReactiveFormsModule,
    GymsRoutingModule,
  ],
  declarations: [
    RegisterMembershipComponent,
    GymProfileComponent,
    GymManagementComponent,
    GymWalletComponent,
    GymPlansComponent,
  ],
  exports: [GymProfileComponent],
})
export class GymModule {}
