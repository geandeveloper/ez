import { map, mergeMap, tap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Store } from '../state/store';
import { UserInfoState, UserState } from './user.state';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountModel } from '../../ezgym/core/models/accout.model';

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

  setActiveAccount(accountName: string) {
    this.setState((state) => ({
      ...state,
      activeAccount: state.userInfo?.accounts.find(a => a.accountName == accountName)
    }))
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
        }),
        mergeMap(() => this.updateUserInfo())
      )
  }

  updateUserInfo(): Observable<UserInfoState> {
    return this.http
      .get<UserInfoState>(`userInfo`)
      .pipe(
        tap(response => {
          this.setState((state) => ({
            ...state,
            activeAccount: state.activeAccount || response.accounts.find(a => a.isDefault),
            userInfo: response
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
        mergeMap(() => this.updateUserInfo()),
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

  updateActiveAccount(updateAccountFn: (account: AccountModel) => AccountModel) {
    this.setState(state => {

      const account = updateAccountFn(state.activeAccount!)

      const newState = {
        ...state,
        activeAccount: account,
        userInfo: {
          ...state.userInfo,
          accounts: state.userInfo?.accounts.map(a => a.id === account.id ? account : a)!
        }
      }

      return newState
    })
  }
}
