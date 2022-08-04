import { Observable, BehaviorSubject } from 'rxjs';

export class Store<T> {
  protected initialState: T

  public store$: Observable<T>;
  private _state$: BehaviorSubject<T>;

  public get state() {
    return this._state$.getValue()
  }

  protected constructor(initialState?: T) {
    this.initialState = initialState || {} as T;
    this._state$ = new BehaviorSubject(this.initialState);
    this.store$ = this._state$.asObservable();
  }

  setState(setStateFn: (state: T) => T): void {
    this._state$.next(setStateFn(this.state))
  }
}