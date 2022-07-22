import { UserStore } from './user.store';
import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private userStore: UserStore
  ) { }

  canActivate(_route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return true;

    if (this.userStore.authenticated) {
    }
    this.router.navigate(['/authentication/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}