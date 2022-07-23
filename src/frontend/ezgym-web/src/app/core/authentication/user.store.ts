import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Store } from '../state/store';
import { UserState } from './user.state';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserStore extends Store<UserState> {

  user: UserState = { id: "", email: "", accessToken: "", authenticated: false }

  constructor(private http: HttpClient) {
    super()

    this.store$.subscribe(userState => {
      this.user = userState
    })
  }

  authenticate(request: { email: string, password: string }) {
    return this.http
      .post<any>("users/token", request)
      .pipe(
        tap(response => {
          this.setState(() => ({
            id: response.userId,
            email: request.email,
            accessToken: response.accessToken,
            authenticated: true
          }))
        })
      )
  }

  refreshToken(): Observable<UserState> {
    return this.http
      .post<any>(`users/refresh-token`, this.user.id)
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
}