import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from "@angular/router";

import { NgForm } from '@angular/forms';
import { BsModalService } from 'ngx-bootstrap/modal';

import { APIwithActionService } from "../shared/APIwithAction.service";
import { GlobalAPIService } from "../shared/GlobalAPI.service";

@Component({
  selector: 'app-copydefaultdata',
  templateUrl: './copydefaultdata.component.html',

})
export class CopydefaultdataComponent implements OnInit {


  public termsAgreed = false
  countrylist = new Array();
  importForm: NgForm;
  model: any
  Testingarealist = new Array();
  TestAreaIDs = new Array();
  ProductTypeList=new Array();
  ProducttypeIDs = new Array();
  ProgramList = new Array();
  ProgramIDs = new Array();
  constructor(
    private _GlobalAPIService: GlobalAPIService,
    private router: Router,
    private modalService: BsModalService, private _APIwithActionService: APIwithActionService) {

    this.model = {    
      importdata: false,
      chktest:false,
      chkproduct:false,
      chkproductusage:false,
      chkdemosettings:false
    }
  
    this._APIwithActionService.getDataList('Site', 'Getcountrylist').subscribe((data) => {

      this.countrylist = data;
    })
    this.getTestingArea();
    this.getproducttype();
    this.getprogramlist();
    
  }

  ngOnInit() {


  }
  getprogramlist()
  {
    this._APIwithActionService.getDataList('MMProgram',"GetAllbyadmin").subscribe((data)=>{
      this.ProgramList=data;
      
      }
      )
  }
  getTestingArea() {
    this._APIwithActionService.getDataList('Test','GetAllbyadmin').subscribe((data) => {
      this.Testingarealist = data
     
    }
    ), err => {
     
    }

  }
  getproducttype() {  
    this._APIwithActionService.getDataList('Product','GetAllbyadmin').subscribe( (data) => {this.ProductTypeList = data
}
    )   ,err=>{  
     
      }  
   
}  
importdata(form: NgForm) {
    let importobject= new Object();
   
        importobject={
          Testingareaids:this.TestAreaIDs,
          Producttypeids:this.ProducttypeIDs,
          Programids:this.ProgramIDs,
          userid:localStorage.getItem("userid"),
          importtest:this.model.chktest,
          importproduct:this.model.chkproduct,
          importproductusage:this.model.chkproductusage,
          importprogram:this.model.chkdemosettings
      
        }
        this._APIwithActionService.postAPI(importobject,'User','Importdefaultdata').subscribe((response)=>{

          this._GlobalAPIService.SuccessMessage("Default Data imported Successfully");
        })
      
    
    
  }


  onChange(ID: any, Ischecked: boolean)
  {
    let index = this.TestAreaIDs.findIndex(x => x === ID);
    if (Ischecked == true) {
      if (index >= 0) 
      {

      }
      else
      {
            this.TestAreaIDs.push(ID)
      }
    }
    else {
      this.TestAreaIDs.splice(index, 1);
    }
  }
  ontypeChange(ID:any,Ischecked:boolean)
  {
    let index = this.ProducttypeIDs.findIndex(x => x === ID);
    if (Ischecked == true) {
      if (index >= 0) {

      }
      else
        {
          this.ProducttypeIDs.push(ID)
         }
    }
    else {
      this.ProducttypeIDs.splice(index, 1);
    }
  }
  onprogramChange(ID:any,Ischecked:boolean)
  {
    let index = this.ProgramIDs.findIndex(x => x === ID);
    if (Ischecked == true) {
      if (index >= 0) {

      }
      else
        {
          this.ProgramIDs.push(ID)
         }
    }
    else {
      this.ProgramIDs.splice(index, 1);
    }
  }
}

