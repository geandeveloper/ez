import { PreLoaderStore } from './pre-loader.store';

import { NgModule } from "@angular/core";
import { PreLoaderComponent } from './pre-loader.component';
import { CommonModule } from '@angular/common';

@NgModule({
  imports: [CommonModule],
  providers: [PreLoaderStore],
  declarations: [PreLoaderComponent],
  exports: [PreLoaderComponent]
})
export class PreLoaderModule { }