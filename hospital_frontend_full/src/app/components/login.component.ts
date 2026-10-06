import { Component } from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent {
  username = '';
  password = '';
  error = '';

  login() {
    console.log('Login attempted:', this.username);
    this.error = 'Login temporarily disabled';
  }
}
