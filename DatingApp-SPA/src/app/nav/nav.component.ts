import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth_services/auth.service';
import { AlertifyService } from '../_services/alertify_services/alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  // going to store the username and password
  model: any = {};

  // authService is already injected into the nav component (no need to sep. inject JWTHelperService)
  constructor(public authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('Logged in Successfully');
    }, error => {
      this.alertify.error(error);
    });
  }

  loggedIn() {
    // this was the old format of the loggedIn method call from the NAV component
    // const token = localStorage.getItem('token');
    // return !!token;

    // new implementation
    return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
    this.alertify.message('Logged Out');
  }
}
