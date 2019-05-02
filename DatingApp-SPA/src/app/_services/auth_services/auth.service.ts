import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // properties
  baseUrl = 'http://localhost:5000/api/auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;

  constructor(private http: HttpClient) {}

  login(model: any) {
    // the post method returns an Observable of the response body as an object
    return this.http.post(this.baseUrl + 'login', model).pipe(
      map((response: any) => {
        // the response from the server containing the "token" object
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          // this is using the jwtHelper methods included with the service
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          // check to see the values of decodedToken
          // console.log(this.decodedToken);
        }
      })
    );
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'register', model);
  }

  loggedIn() {
    // first get the jwt token from localStorage. In this project the token is named 'token'
    const token = localStorage.getItem('token');
    // isTokenExpired returns a boolen of TRUE if the token is expired, missing, or not a jwt token
    return !this.jwtHelper.isTokenExpired(token);
  }
}
