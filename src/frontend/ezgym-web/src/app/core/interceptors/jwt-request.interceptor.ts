import { UserStore } from './../authentication/user.store';
import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class JwtRequestInterceptor implements HttpInterceptor {
  constructor(private userStore: UserStore) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.userStore.user.authenticated) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${this.userStore.user.accessToken}`
        }
      });

    } else {

    }

    return next.handle(request);
  }
}