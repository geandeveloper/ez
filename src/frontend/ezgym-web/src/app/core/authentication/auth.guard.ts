import { UserStore } from './user.store';
import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { map } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private userStore: UserStore
  ) { }

  canActivate(_route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

    debugger
    const user = this.userStore.user;

    if (user.authenticated)
      return true;
      
    return this.userStore
      .refreshToken({ userId: user.id })
      .pipe(
        map(user => user.authenticated)
      )
  }
}