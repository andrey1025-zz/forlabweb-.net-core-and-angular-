import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { NgForm } from '@angular/forms';
import { APIwithActionService } from "../../shared/APIwithAction.service";
import { GlobalAPIService } from "../../shared/GlobalAPI.service";
@Component({
  selector: 'app-forgot',
  templateUrl: './forgot.component.html',
  styles: []
})
export class ForgotComponent implements OnInit {
  model: any
  forgetForm: NgForm;
  constructor(private router: Router, private _APIwithActionService: APIwithActionService, private _GlobalAPIService: GlobalAPIService) {


    this.model = {
      email: '',
    }
  }

  ngOnInit() {
  }

  submit() {
    this._APIwithActionService.getDatabyID(this.model.email, "User", "Resetpassword").subscribe((data) => {
      this._GlobalAPIService.SuccessMessage("Please Check your Email to Reset Password");
      this.model.email = '';
    })
  }
}
