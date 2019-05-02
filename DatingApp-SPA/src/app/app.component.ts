import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth_services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  jwtHelper = new JwtHelperService();
  title = 'app';

  constructor(private authService: AuthService) {
  }

  // put the lifecycle hook onInit here because at the nav component level,
  // when the user refreshes the page the decoded token proerty is not persisted. The optional ? in the HTML
  // prevents an error, but the name is still missing. Setting the decoded token here at the app comp.
  // resolves the presistance error
  ngOnInit(): void {
    const token = localStorage.getItem('token');
    if (token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }
  }
}
