import { ModalComponent } from './modal.component';
import { MatDialogModule } from '@angular/material/dialog';
import { ModalStore } from './modal.store';
import { NgModule } from "@angular/core";

@NgModule({
  providers: [ModalStore],
  imports: [MatDialogModule],
  declarations: [ModalComponent]
})
export class ModalModule { }