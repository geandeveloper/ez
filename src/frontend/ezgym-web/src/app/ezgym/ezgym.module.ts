import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';

import { EzGymComponent } from './ezgym.component';

import { EzGymRoutingModule } from "./ezgym-routing.module";
import { BrowserModule } from '@angular/platform-browser';
import { ProfileComponent } from './profile/profile.component';
import { MatDialogModule } from "@angular/material/dialog";


@NgModule({
  declarations: [
    EzGymComponent,
    ProfileComponent
  ],
  imports: [
    FormsModule,
    BrowserModule,
    MatDialogModule,
    ReactiveFormsModule,
    EzGymRoutingModule,
    MatAutocompleteModule,
    MatFormFieldModule,
  ],
  exports: [
    EzGymComponent
  ]
})
export class EzGymModule { }