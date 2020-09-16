import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { APIwithActionService } from 'app/shared/APIwithAction.service';
import { Router } from '@angular/router';
import { GlobalAPIService } from 'app/shared/GlobalAPI.service';
import { BsModalService, BsDropdownConfig } from 'ngx-bootstrap';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'register-modal',
  templateUrl: './registermodal.component.html',
  styleUrls: ['./registermodal.component.css'],
  providers: [{ provide: BsDropdownConfig, useValue: { isAnimated: true, autoClose: true } }]
})
export class RegisterModalComponent {

  @Output() close = new EventEmitter()
  countrylist = new Array();
  model: any
  registerForm: NgForm;
  selectedCountry: any = "Country";
  Testingarealist = new Array();
  TestAreaIDs = new Array();
  ProductTypeList = new Array();
  ProducttypeIDs = new Array();
  ProgramList = new Array();
  ProgramIDs = new Array();

  constructor(private _GlobalAPIService: GlobalAPIService, private router: Router, private modalService: BsModalService,
    private _APIwithActionService: APIwithActionService) {

    this.model = {
      fullname: '',
      username: '',
      email: '',
      password: '',
      passwordConfirm: '',
      organization: '',
      countryid: '',
      importdata: false,
      chktest: false,
      chkproduct: false,
      chkproductusage: false,
      chkdemosettings: false,
      globalregion: '',
      subscription: false,
      terms: false
    }
    localStorage.removeItem("jwt");
    localStorage.removeItem("username");
    localStorage.setItem("userid", "0");
    this._APIwithActionService.getDataList('Site', 'Getcountrylist').subscribe((data) => {

      this.countrylist = data;
    })
    // this.getTestingArea();
    // this.getproducttype();
    // this.getprogramlist();
  }

  Getregion(id: any, value: any) {
    this.selectedCountry = value;
    this._APIwithActionService.getDatabyID(id, 'User', 'Getglobalregion').subscribe((data) => {
      this.model.globalregion = data.res
    });
  }

  register(form: NgForm) {
    let importobject = new Object();

    this._APIwithActionService.postAPI(form.value, 'User', 'Saveuser').subscribe((response) => {
      if (JSON.parse(response["_body"])[1] == 0) {
        this._GlobalAPIService.FailureMessage(JSON.parse(response["_body"])[0])
      }
      else {
        if (JSON.parse(response["_body"])[0] == "Something went wrong") {
          this._GlobalAPIService.FailureMessage(JSON.parse(response["_body"])[0])
        }
        else {
          importobject = {
            Testingareaids: this.TestAreaIDs,
            Producttypeids: this.ProducttypeIDs,
            Programids: this.ProgramIDs,
            userid: JSON.parse(response["_body"])[1],
            importtest: this.model.chktest,
            importproduct: this.model.chkproduct,
            importproductusage: this.model.chkproductusage,
            importprogram: this.model.chkdemosettings
          }
          this.close.emit(true);
          this._APIwithActionService.postAPI(importobject, 'User', 'Importdefaultdata').subscribe((response) => { })
          this._GlobalAPIService.SuccessMessage("You are successfully registered for ForLab+,Verification link has been send to your email to Activate your account");
          this.router.navigate(["/"]);
        }
      }
    }, err => {

    });
  }
}
