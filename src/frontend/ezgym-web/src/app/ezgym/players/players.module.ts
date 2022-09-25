import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { CheckinComponent } from './checkin/checkin.component';

@NgModule({
  imports: [CommonModule],
  declarations: [CheckinComponent],
  exports: [CheckinComponent],
})
export class PlayersModule {}
