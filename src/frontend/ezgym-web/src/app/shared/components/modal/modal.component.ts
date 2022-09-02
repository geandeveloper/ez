import { Component, Inject } from '@angular/core';
import { ModalState } from './modal.state';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss']
})
export class ModalComponent {
  constructor( @Inject(MAT_DIALOG_DATA) public state: ModalState
  ) {
  }

}
