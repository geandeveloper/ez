import { Component } from '@angular/core';
import { Store } from 'src/app/core/state/store';

interface ComponentState {
  open: boolean;
}

@Component({
  selector: 'bottom-modal',
  templateUrl: './bottom-modal.component.html',
  styleUrls: ['./bottom-modal.component.scss'],
})
export class BottomModalComponent extends Store<ComponentState> {
  constructor() {
    super({
      open: false,
    });
  }

  public open() {
    this.setState((state) => ({ ...state, open: true }));
  }

  public close() {
    this.setState((state) => ({ ...state, open: false }));
  }
}
