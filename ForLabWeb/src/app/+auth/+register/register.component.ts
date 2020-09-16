import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from "@angular/router";

import { NgForm } from '@angular/forms';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { APIwithActionService } from "../../shared/APIwithAction.service";
import { GlobalAPIService } from "../../shared/GlobalAPI.service";
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styles: [`.ng-invalid.ng-touched:not(form){    border-color: #c70f0f;
    background-color: #f3e7e9;}`]
})
export class RegisterComponent implements OnInit {


  bsModalRef: BsModalRef;
  public termsAgreed = false
  countrylist = new Array();
  registerForm: NgForm;
  model: any
  Testingarealist = new Array();
  TestAreaIDs = new Array();
  ProductTypeList = new Array();
  ProducttypeIDs = new Array();
  ProgramList = new Array();
  ProgramIDs = new Array();
  constructor(
    private _GlobalAPIService: GlobalAPIService,
    private router: Router,
    private modalService: BsModalService, private _APIwithActionService: APIwithActionService) {

    this.model = {
      firstname: '',
      lastname: '',
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
    this.getTestingArea();
    this.getproducttype();
    this.getprogramlist();

  }
  Getregion(id: any) {
    this._APIwithActionService.getDatabyID(id, 'User', 'Getglobalregion').subscribe((data) => {
      this.model.globalregion = data.res
    });
  }
  ngOnInit() {


  }
  getprogramlist() {
    this._APIwithActionService.getDataList('MMProgram', "GetAllbyadmin").subscribe((data) => {
      this.ProgramList = data;

    }
    )
  }
  getTestingArea() {
    this._APIwithActionService.getDataList('Test', 'GetAllbyadmin').subscribe((data) => {
      this.Testingarealist = data

    }
    ), err => {

    }

  }
  getproducttype() {
    this._APIwithActionService.getDataList('Product', 'GetAllbyadmin').subscribe((data) => {
      this.ProductTypeList = data

    }
    ), err => {
    }

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

          this._APIwithActionService.postAPI(importobject, 'User', 'Importdefaultdata').subscribe((response) => { })
          this._GlobalAPIService.SuccessMessage("You are successfully registered for ForLab+,Verification link has been send to your email to Activate your account");
          this.router.navigate(["/"]);
        }
      }
    }, err => {

    });
  }

  openModal(event, template: TemplateRef<any>) {
    event.preventDefault();
    this.bsModalRef = this.modalService.show(template);
  }

  onTermsAgree() {
    this.termsAgreed = true
    this.bsModalRef.hide()
  }

  onTermsClose() {
    this.bsModalRef.hide()
  }

  onChange(ID: any, Ischecked: boolean) {
    let index = this.TestAreaIDs.findIndex(x => x === ID);
    if (Ischecked == true) {
      if (index >= 0) {

      }
      else {
        this.TestAreaIDs.push(ID)
      }
    }
    else {
      this.TestAreaIDs.splice(index, 1);
    }
  }
  ontypeChange(ID: any, Ischecked: boolean) {
    let index = this.ProducttypeIDs.findIndex(x => x === ID);
    if (Ischecked == true) {
      if (index >= 0) {

      }
      else {
        this.ProducttypeIDs.push(ID)
      }
    }
    else {
      this.ProducttypeIDs.splice(index, 1);
    }
  }
  onprogramChange(ID: any, Ischecked: boolean) {
    let index = this.ProgramIDs.findIndex(x => x === ID);
    if (Ischecked == true) {
      if (index >= 0) {

      }
      else {
        this.ProgramIDs.push(ID)
      }
    }
    else {
      this.ProgramIDs.splice(index, 1);
    }
  }
}
