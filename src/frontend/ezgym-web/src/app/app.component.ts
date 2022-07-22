import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ezgym-web';
  signUpMode = false;

  signUpModeToggle() {
    setTimeout(() => {
      this.signUpMode = true
    }, 100)
  }

}
