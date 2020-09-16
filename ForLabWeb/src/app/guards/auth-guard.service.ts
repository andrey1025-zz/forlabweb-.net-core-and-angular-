import { JwtHelper } from 'angular2-jwt';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private jwtHelper: JwtHelper, private router: Router) {
  }
  canActivate() {
 
    if( localStorage.getItem("userid")=="0")
    {
      this.router.navigate(["/"]);
      return false;
    }
    // else
    // {
    //   this.router.navigate(["/Dashboard"]);
    // }
    var token = localStorage.getItem("jwt");
 
    if (token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }
    localStorage.removeItem("jwt");
    localStorage.removeItem("username");
    localStorage.setItem("userid","0");
    this.router.navigate(["/"]);
    return false;
  }
}