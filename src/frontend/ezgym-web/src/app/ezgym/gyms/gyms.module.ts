
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MatStepperModule } from '@angular/material/stepper';
import { MatTabsModule } from '@angular/material/tabs';
import { ImageCropperModule } from 'ngx-image-cropper';

import { GymProfileComponent } from './gym-profile/gym-profile.component';
import { GymsRoutingModule } from './gyms-routing.module';
import { GymManagementComponent } from './management/gym-management.component';
import { GymPlansComponent } from './management/gym-plans/gym-plans.component';
import { GymWalletComponent } from './management/gym-wallet/gym-wallet.component';
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
        GymPlansComponent
    ],
    exports: [
        GymProfileComponent
    ],
    providers: [],
})
export class GymModule { }
