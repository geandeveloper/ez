
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { MatStepperModule } from '@angular/material/stepper';
import { ImageCropperModule } from 'ngx-image-cropper';

import { GymProfileComponent } from './gym-profile/gym-profile.component';
import { GymsRoutingModule } from './gyms-routing.module';
import { RegisterMembershipComponent } from './register-membership/register-membership.component';

@NgModule({
    imports: [
        ImageCropperModule,
        MatStepperModule,
        ReactiveFormsModule,
        GymsRoutingModule
    ],
    declarations: [
        RegisterMembershipComponent,
        GymProfileComponent,
    ],
    exports: [
        GymProfileComponent
    ],
    providers: [],
})
export class GymModule { }
