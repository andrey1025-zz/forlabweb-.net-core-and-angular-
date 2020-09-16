import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { Router } from "@angular/router";
import { NgForm } from '@angular/forms';
import { APIwithActionService } from "../../shared/APIwithAction.service";
import { GlobalAPIService } from "../../shared/GlobalAPI.service";
import { ngxLoadingAnimationTypes, NgxLoadingComponent } from 'ngx-loading';
import { ModalDirective } from "ngx-bootstrap";
import * as ftpClient from 'ftp-client';
const PrimaryWhite = '#ffffff';
const SecondaryGrey = '#ccc';
const PrimaryRed = '#dd0031';
const SecondaryBlue = '#006ddd';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {
  loginForm: NgForm;
  invalidLogin: boolean;
  @ViewChild('ngxLoading') ngxLoadingComponent: NgxLoadingComponent;
  @ViewChild('customLoadingTemplate') customLoadingTemplate: TemplateRef<any>;
  @ViewChild('lgModal1') public lgModal1: ModalDirective;
  public ngxLoadingAnimationTypes = ngxLoadingAnimationTypes;

  public primaryColour = PrimaryRed;
  public secondaryColour = SecondaryBlue;
  public coloursEnabled = false;
  public loadingTemplate: TemplateRef<any>;
  public config = { animationType: ngxLoadingAnimationTypes.none, primaryColour: this.primaryColour, secondaryColour: this.secondaryColour, tertiaryColour: this.primaryColour, backdropBorderRadius: '3px' };
  public loading = false;
  constructor(private router: Router, private _APIwithActionService: APIwithActionService, private _GlobalAPIService: GlobalAPIService) { }

  ngOnInit() {
  }

  login(form: NgForm) {
    this.loading = true;
    this._APIwithActionService.postAPI(form.value, 'User', 'Authenticate').subscribe((response) => {


      let body = JSON.parse(response["_body"]);
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
      this.loading = false;
      if (localStorage.getItem("logincnt") == "0") {

        this._APIwithActionService.getDatabyID(body.id, "User", "updatelogincount").subscribe((data) => {

        })
        this.router.navigate(["Forecast/RecentForecast"]);
        ///this.lgModal1.show();
        localStorage.setItem("logincnt", "1");
      }
      else {
        this.router.navigate(["Forecast/RecentForecast"]);
      }



    }, err => {
      this.invalidLogin = true;
    });
  }
  Openimportdata() {
    this.router.navigate(["/CopyDefaultData"]);
  }
  Downloadwindowversion() {

  }
}



