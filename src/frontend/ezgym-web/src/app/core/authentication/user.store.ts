import { map, tap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Store } from '../state/store';
import { UserState } from './user.state';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserStore extends Store<UserState> {

  user: UserState

  constructor(private http: HttpClient) {
    super({ id: "", accessToken: "", authenticated: false })

    this.user = this.initialState;

    this.store$.subscribe(userState => {
      this.user = {
        ...userState,
      }
    })
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
            authenticated: true
          }))
        })
      )
  }

  refreshToken(): Observable<UserState> {
    return this.http
      .post<any>(`users/refresh-token`, {})
      .pipe(
        tap(response => {
          this.setState((state) => ({
            ...state,
            authenticated: true,
            accessToken: response.accessToken
          }))
        }),
        map(() => this.user)
      )
  }

  revokeToken(): Observable<UserState> {
    return this.http
      .post<any>(`users/revoke-token`, {})
      .pipe(
        tap(() => {
          this.setState(() => ({ ...this.initialState }))
        }),
        map(() => this.initialState)
      )
  }
}
