import { map, tap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Store } from '../state/store';
import { UserState } from './user.state';
import { Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';
import { Preferences } from '@capacitor/preferences';


@Injectable({
  providedIn: 'root'
})
export class UserStore extends Store<UserState> {

  constructor(private http: HttpClient) {
    super({ id: "", accessToken: "", refreshToken: ":", authenticated: false })

    this.store$.subscribe(async userState => {
      if (userState.authenticated)
        await Preferences.set({
          key: 'user',
          value: JSON.stringify(userState)
        });
    })

  }

  SyncUnser(): Observable<UserState> {
    return from(new Promise(async (resolve) => {
      const user = await Preferences
        .get({ key: 'user' });
      if (user.value)
        resolve(JSON.parse(user.value) as UserState);
      resolve(this.initialState);
    }))
      .pipe(
        map(value => value as UserState),
        tap(userState => this.setState(_ => ({ ...userState })))
      )
  }


  createUser(request: UserState) {
    return this.http
      .post<UserState>("users", request)
  }


  authenticate(request: { userName: string, password: string }) {
    return this.http
      .post<any>("users/token", request)
      .pipe(
        tap(response => {
          this.setState(() => ({
            id: response.userId,
            accessToken: response.accessToken,
            refreshToken: response.refreshToken,
            authenticated: true
          }))
        })
      )
  }

  refreshToken(): Observable<UserState> {
    return this.http
      .post<any>(`users/refresh-token`, { refreshToken: this.state.refreshToken })
      .pipe(
        tap(response => {
          this.setState((state) => ({
            ...state,
            authenticated: true,
            accessToken: response.accessToken,
            refreshToken: response.refreshToken,
          }))
        })
      )
  }

  revokeToken(): Observable<UserState> {
    return this.http
      .post<any>(`users/revoke-token`, { refreshToken: this.state.refreshToken })
      .pipe(
        tap(() => {
          this.setState(() => ({ ...this.initialState }))
        }),
        map(() => this.initialState)
      )
  }
}
