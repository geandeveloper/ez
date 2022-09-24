import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { BottomModalComponent } from './bottom-modal/bottom-modal.component';

@NgModule({
  imports: [CommonModule],
  declarations: [BottomModalComponent],
  exports: [BottomModalComponent],
})
export class SharedComponentsModule {}
