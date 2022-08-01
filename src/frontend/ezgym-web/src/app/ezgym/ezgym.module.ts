import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';

import { EzGymComponent } from './ezgym.component';

import { EzGymRoutingModule } from "./ezgym-routing.module";
import { BrowserModule } from '@angular/platform-browser';
import { ProfileComponent } from './profile/profile.component';
import { MatDialogModule } from "@angular/material/dialog";
import { ImageCropperModule } from "ngx-image-cropper";
import { EditProfileComponent } from "./profile/edit-profile/edit-profile.component";


@NgModule({
  declarations: [
    EzGymComponent,
    ProfileComponent,
    EditProfileComponent
  ],
  imports: [
    FormsModule,
    BrowserModule,
    MatDialogModule,
    ReactiveFormsModule,
    EzGymRoutingModule,
    MatAutocompleteModule,
    MatFormFieldModule,
    ImageCropperModule,
  ],
  exports: [
    EzGymComponent
  ]
})
export class EzGymModule { }