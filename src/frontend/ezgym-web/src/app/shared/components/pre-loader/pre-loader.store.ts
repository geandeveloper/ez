import { PreLoaderState } from './pre-loader.state';
import { Store } from "src/app/core/state/store";
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';

@Injectable()
export class PreLoaderStore extends Store<PreLoaderState> {

  showed$ = this.store$.pipe(map(state => state.show))

  constructor() {
    super({
      show: false
    })
  }

  show() {
    this.setState(state => ({
      ...state,
      show: true
    }))
  }

  close() {
    this.setState(state => ({
      ...state,
      show: false
    }))
  }
}