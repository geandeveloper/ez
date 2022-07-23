import { UserStore } from './user.store';
import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { catchError, map, of, pipe, tap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private userStore: UserStore
  ) { }

  canActivate(_route: ActivatedRouteSnapshot, _state: RouterStateSnapshot) {
    const user = this.userStore.user;
    
    if (user.authenticated)
      return true;

    return this.userStore
      .refreshToken()
      .pipe(
        map(user => user.authenticated),
        tap(authenticated => {
          if (!authenticated)
            this.router.navigate(['ezidentity/login']);
        }),
        catchError((_error) => {
          this.router.navigate(['ezidentity/login']);
          return of(false)
        })
      )
  }
}