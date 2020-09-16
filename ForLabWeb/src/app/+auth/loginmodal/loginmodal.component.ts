import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { APIwithActionService } from 'app/shared/APIwithAction.service';
import { GlobalAPIService } from 'app/shared/GlobalAPI.service';

@Component({
  selector: 'login-modal',
  templateUrl: './loginmodal.component.html',
  styleUrls: ['./loginmodal.component.css']
})
export class LoginModalComponent {
  loginForm: NgForm;
  invalidLogin: boolean;
  loading = false;

  @Output() close = new EventEmitter()
  @Output() onForgotpwd = new EventEmitter()

  constructor(private router: Router, private _APIwithActionService: APIwithActionService, private _GlobalAPIService: GlobalAPIService) { }

  login(form: NgForm) {
    this.loading = true;
    this._APIwithActionService.postAPI(form.value, 'User', 'Authenticate').subscribe((response) => {
      let body = JSON.parse(response["_body"]);
      this.loading = false;
      if (body && response.status == 200) {
        this.close.emit(true);
        if (body.emailverify == false) {
          this._GlobalAPIService.FailureMessage("Email-Id is not verify .Please verify the email to login");
          return;
        }
        let token = body.token
        let username = body.firstName + " " + body.lastName
        localStorage.setItem("username", username);
        localStorage.setItem("jwt", token);
        localStorage.setItem("userid", body.id);
        localStorage.setItem("countryid", body.countryId);
        localStorage.setItem("role", body.role);
        localStorage.setItem("logincnt", body.logincnt);
        this.invalidLogin = false;
        let Countryid: any
        if (localStorage.getItem("role") == "admin") {
          localStorage.setItem("countryid", "0");
        }
        else {

        }
        if (localStorage.getItem("logincnt") == "0") {
          this._APIwithActionService.getDatabyID(body.id, "User", "updatelogincount").subscribe((data) => {

          })
          this.router.navigate(["Dashboard"]);
          localStorage.setItem("logincnt", "1");
        }
        else {
          this.router.navigate(["Dashboard"]);
        }
      } else {
        this._GlobalAPIService.FailureMessage("Credential is incorrect .Please check the informations again");
        return;
      }
    }, err => {
      this.loading = false;
      this.invalidLogin = true;
    });
  }
}
