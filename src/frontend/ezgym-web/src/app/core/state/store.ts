import { Observable, BehaviorSubject } from 'rxjs';

export class Store<T> {
  protected store$: Observable<T>;
  private _state$: BehaviorSubject<T>;

  protected constructor(initialState?: T) {
    this._state$ = new BehaviorSubject(initialState || {} as T);
    this.store$ = this._state$.asObservable();
  }

  setState(setStateFn: (state: T) => T): void {
    this._state$.next(setStateFn(this._state$.getValue()));
  }
}