import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { UserStore } from './core/authentication/user.store';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  animations: [
  ]
})
export class AppComponent {
  title = 'ezgym-web';
  signUpMode = false;

  signUpModeToggle() {
    setTimeout(() => {
      this.signUpMode = true
    }, 100)
  }

  prepareRoute(outlet: RouterOutlet) {
    return outlet && outlet.activatedRouteData && outlet.activatedRouteData['animation'];
  }

}
