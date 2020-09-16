import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from "@angular/router";

import { NgForm } from '@angular/forms';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { APIwithActionService } from "../shared/APIwithAction.service";
import { GlobalAPIService } from "../shared/GlobalAPI.service";

@Component({
  selector: 'app-adminuser',
  templateUrl: './adminuser.component.html'
})
export class adminuserComponent implements OnInit {
  bsModalRef: BsModalRef;
  
  countryGloballist = new Array();
  adminForm: NgForm;
  model: any
  // Testingarealist = new Array();
  // TestAreaIDs = new Array();
  // ProductTypeList = new Array();
  // ProducttypeIDs = new Array();
  // ProgramList = new Array();
  // ProgramIDs = new Array();
  constructor( private _GlobalAPIService: GlobalAPIService,
    private router: Router,
    private modalService: BsModalService, private _APIwithActionService: APIwithActionService)
     {

      this.model = {
        firstname: '',
        lastname: '',
        username: '',
        email: '',
        password: '',
        passwordConfirm: '',
        organization: '',
        globalregionid: ''
        // importdata: false,
        // chktest: false,
        // chkproduct: false,
        // chkproductusage: false,
        // chkdemosettings: false,
        // globalregion:''
      }
      localStorage.removeItem("jwt");
      localStorage.removeItem("username");
      localStorage.setItem("userid", "0");
      this._APIwithActionService.getDataList('User', 'Getallglobalregions').subscribe((data) => {
  
        this.countryGloballist = data;
    
      })
   
  

      }

  ngOnInit() {
  }
  register(form: NgForm) {
   





    this._APIwithActionService.postAPI(form.value, 'User', 'Saveuser').subscribe((response) => {
      if (JSON.parse(response["_body"])[1] == 0) {
        this._GlobalAPIService.FailureMessage(JSON.parse(response["_body"])[0])
      }
      else {
        if (JSON.parse(response["_body"])[0] == "Something went wrong") {
          this._GlobalAPIService.FailureMessage(JSON.parse(response["_body"])[0])
        }
        else {
          // importobject = {
          //   Testingareaids: this.TestAreaIDs,
          //   Producttypeids: this.ProducttypeIDs,
          //   Programids: this.ProgramIDs,
          //   userid: JSON.parse(response["_body"])[1],
          //   importtest: this.model.chktest,
          //   importproduct: this.model.chkproduct,
          //   importproductusage: this.model.chkproductusage,
          //   importprogram: this.model.chkdemosettings

          // }

          // this._APIwithActionService.postAPI(importobject, 'User', 'Importdefaultdata').subscribe((response) => { })
          // this._GlobalAPIService.SuccessMessage("You are successfully registered for FORLAB,Verification link has been send to your email to Activate your account");
          // this.router.navigate(["/"]);
          this._GlobalAPIService.SuccessMessage("Global Admin successfully created for ForLab+,Verification link has been send to email to Activate user account");
     //     this.router.navigate(["/"]);
        }
      }
    }, err => {

    });
  }
}
