import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { APIwithActionService } from 'app/shared/APIwithAction.service';
import { GlobalAPIService } from 'app/shared/GlobalAPI.service';

@Component({
  selector: 'forgotpwd-modal',
  templateUrl: './forgotpwdmodal.component.html',
  styleUrls: ['./forgotpwdmodal.component.css']
})
export class ForgotpwdModalComponent {
  forgotpwdForm: NgForm;
  model: any

  @Output() close = new EventEmitter()
  @Output() onLogin = new EventEmitter()

  constructor(private router: Router, private _APIwithActionService: APIwithActionService, private _GlobalAPIService: GlobalAPIService) {
    this.model = { email: '' }
  }

  submit() {
    this._APIwithActionService.getDatabyID(this.model.email, "User", "Resetpassword").subscribe((data) => {
      this._GlobalAPIService.SuccessMessage("Please Check your Email to Reset Password");
      this.model.email = '';
    })
  }
}
