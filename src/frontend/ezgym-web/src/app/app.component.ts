import { Component, NgZone } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { App, URLOpenListenerEvent } from '@capacitor/app';

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

  constructor(private router: Router, private zone: NgZone) {
  }

  signUpModeToggle() {
    setTimeout(() => {
      this.signUpMode = true
    }, 100)
  }

  prepareRoute(outlet: RouterOutlet) {
    return outlet && outlet.activatedRouteData && outlet.activatedRouteData['animation'];
  }


}
