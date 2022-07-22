import { tap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Store } from '../state/store';
import { UserState } from './user.state';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserStore extends Store<UserState> {

  authenticated: boolean = false
  currentUser: UserState = { email: '', id: '', token: '' }

  constructor(private http: HttpClient) {
    super()

    this.store$.subscribe(user => {
      if (user) {
        this.authenticated = user?.token != null
        this.currentUser = user

        localStorage.setItem("session", JSON.stringify(user))
      }
    })

    if (localStorage.getItem("session") != null) {
      this.setState(() => JSON.parse(localStorage.getItem("session")!))
    }

  }

  authenticate(request: { email: string, password: string }) {
    return this.http
      .post<UserState>("user/login", request)
      .pipe(
        tap(user => {
          this.setState(() => user)
        })
      )
  }
}