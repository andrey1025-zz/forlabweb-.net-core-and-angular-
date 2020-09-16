import { Component, OnInit, TemplateRef, ViewChild, Host, Input, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl, FormArray } from '@angular/forms';
import { APIwithActionService } from "../../shared/APIwithAction.service";
import { GlobalAPIService } from "../../shared/GlobalAPI.service";
import { ActivatedRoute } from '@angular/router';
//import { ConsumptionAddComponent } from "../consumption-add/consumption-add.component";
import { ModalDirective } from "ngx-bootstrap";
import { NumberFormatStyle } from '@angular/common';
import { ngxLoadingAnimationTypes, NgxLoadingComponent } from 'ngx-loading';
const PrimaryWhite = '#ffffff';
const SecondaryGrey = '#ccc';
const PrimaryRed = '#dd0031';
const SecondaryBlue = '#006ddd';

@Component({
  selector: 'app-consumptiondatausage1',
  templateUrl: './consumptiondatausage1.component.html',
  styles: ['.table tr.active td {   background-color:#123456 !important;    color: white;  }'],
})

export class Consumptiondatausage1Component implements OnInit {
  public loading = false;
  selectedRow: number;
  datausage1: FormGroup;
  SiteList = new Array();
  Selectedsite = new Array();
  selectedsiteid: string = "0";
  checksite: boolean = false;
  disaddproduct: boolean = true;
  disactualconsumption: boolean = true;
  disremoveconsumption: boolean = true;
  controlArray = new Array();
  datalist = new Array();
  colspan: number = 0;
  makearry:boolean=true;
  @ViewChild('ngxLoading') ngxLoadingComponent: NgxLoadingComponent;
  @ViewChild('customLoadingTemplate') customLoadingTemplate: TemplateRef<any>;
  public ngxLoadingAnimationTypes = ngxLoadingAnimationTypes;

  public primaryColour = PrimaryRed;
  public secondaryColour = SecondaryBlue;
  public coloursEnabled = false;
  public loadingTemplate: TemplateRef<any>;
  public config = { animationType: ngxLoadingAnimationTypes.none, primaryColour: this.primaryColour, secondaryColour: this.secondaryColour, tertiaryColour: this.primaryColour, backdropBorderRadius: '3px' };
  forecastsiteproductid: number = 0;
  @Input() model:any;
  @Input() model1:any;
  @Input() model3:any;
  @Input() RecforecastID1:any;
  forecastid: number = 0;
  @Output()
  nextStep = new EventEmitter<string>();
  constructor(private _avRoute: ActivatedRoute, private _fb: FormBuilder, private _APIwithActionService: APIwithActionService, private _GlobalAPIService: GlobalAPIService) {
    if (this._avRoute.snapshot.params["id"]) {
      this.forecastid = this._avRoute.snapshot.params["id"];
    }
    if (this.RecforecastID1>0)
    {
      this.forecastid = this.RecforecastID1
    }
    this._APIwithActionService.getDatabyID(localStorage.getItem("countryid"),'Site','GetAll').subscribe((data) => {
      this.SiteList = data.aaData;
    });
 
    this._APIwithActionService.GetDataUsage.subscribe((data: any) => {
            this.makeformarray(data)
    }

    )
 




    this._APIwithActionService.getDatabyID(this.forecastid, "Consumption", "Getforecastsite").subscribe((data) => {
      this.Selectedsite = data;
    })
  }
  openconsumption() {
    this.model1.show()
  }

  openmodel() {
    this.model.show()
  }
  openimportmodel()
  {
    this.model3.show()
  }
 Previousclick()
 {
   this.nextStep.emit('step1,P,'+this.forecastid)
 }
 nextclick()
 {
  this.nextStep.emit('step3,N,'+this.forecastid)
 }
  ngOnInit() {

    if (this.RecforecastID1>0)
    {
          this.forecastid = this.RecforecastID1;
    }
    this.datausage1 = this._fb.group({

      selectedsiteList: this._fb.array([]),

      _datausage: this._fb.array([])




    })
   
  }





  initdatausge() {
    // Initialize add Box form field
    let boxForm: FormGroup = this._fb.group({
      productid: 0,
      productname: [{ value: '', disabled: true }],
      value: this._fb.array([])
    });
    return boxForm;

  }

  adddatausge() {

    (<FormArray>this.datausage1.controls["_datausage"]).push(
      this.initdatausge()
    );
    // get box length for box name like box 1,box 2 in sidebar boxes combo list
    let connection_boxes_length = (<FormArray>(
      this.datausage1.controls["_datausage"]
    )).length;

  }
  selectallsites(isChecked: boolean) {
    const selectedsiteList = <FormArray>this.datausage1.controls.selectedsiteList;
    if (isChecked) {
      if (selectedsiteList.controls.length > 0) {
        for (let index1 = selectedsiteList.controls.length - 1; index1 >= 0; index1--) {
          selectedsiteList.removeAt(index1);


        }
        this.Selectedsite.splice(0, this.Selectedsite.length)
      }
      this.checksite = true;
      for (let index1 = 0; index1 < this.SiteList.length; index1++) {
        this.Selectedsite.push({
          siteID: this.SiteList[index1].siteID,
          siteName: this.SiteList[index1].siteName
        })
        this.addsite();
        (<FormGroup>(
          (<FormArray>this.datausage1.controls["selectedsiteList"]).controls[
          index1
          ]
        )).patchValue({
          SiteID: this.SiteList[index1].siteID,
          SiteName: this.SiteList[index1].siteName
        })
      }

    }
    else {
      this.checksite = false;
      if (selectedsiteList.controls.length > 0) {
        for (let index1 = selectedsiteList.controls.length - 1; index1 >= 0; index1--) {
          selectedsiteList.removeAt(index1);
        }
        this.Selectedsite.splice(0, this.Selectedsite.length)
      }
    }
  }
  initsite() {
    let site: FormGroup = this._fb.group({

      SiteID: 0,
      SiteName: ''

    });
    return site;
  }
  addsite() {
    (<FormArray>this.datausage1.controls["selectedsiteList"]).push(
      this.initsite()
    );
  }
  onsiteChange(data: any, isChecked: boolean) {
    const selectedsiteList = <FormArray>this.datausage1.controls.selectedsiteList;
    let index: number
    let regionids: string = "";
    if (isChecked) {

      index = selectedsiteList.length;
      let siteindex = this.Selectedsite.findIndex(x => x.siteID === data.siteID)
      if (siteindex < 0) {
        this.Selectedsite.push({
          siteID: data.siteID,
          siteName: data.siteName
        })
        this.addsite();
        (<FormGroup>(
          (<FormArray>this.datausage1.controls["selectedsiteList"]).controls[
          index
          ]
        )).patchValue({
          SiteID: data.siteID,
          SiteName: data.siteName
        })
      }


    } else {
      for (let index1 = 0; index1 < selectedsiteList.controls.length; index1++) {
        const element = <FormGroup>selectedsiteList.controls[index1];
        if (element.controls.SiteID.value == data.siteID) {
          selectedsiteList.removeAt(index1);
          let siteindex = this.Selectedsite.findIndex(x => x.siteID == data.siteID)

          if (siteindex >= 0) {
            this.Selectedsite.splice(siteindex, 1)
          }
          break;
        }

      }

    }
    if (this.Selectedsite.length == 0) {
      this.disaddproduct = true;
    }
   
  }
  selectsitefordatausage(data: any, index) {
    this.selectedRow = index;
    this.selectedsiteid = data.siteID;
 
    if (this.selectedsiteid != "0" ) {
this.loading=true;
      this._APIwithActionService.getDatabyID(this.forecastid, "Consumption", "Bindforecastsiteproduct", "siteorcatid=" + this.selectedsiteid).subscribe((data) => {
       this.makeformarray(data);

       this.loading=false;
        // let _datausage = <FormArray>this.datausage1.controls["_datausage"];
        // for (let index = 0; index < data.datausage.length; index++) {
        //   _datausage.push(this._fb.group(data.datausage[index]))


        // }
     
      })
      this.disaddproduct = false;
    }
  }
  Enableconsumptionbtn(i: any, j: any, duration: string) {
    let aa = (<FormGroup>((<FormArray>(
      (<FormGroup>(
        (<FormArray>this.datausage1.controls["_datausage"])
          .controls[i]
      )).controls["value"]
    )).controls[j])).controls[duration + "-id"].value
   
    this.forecastsiteproductid = aa;
    this.disactualconsumption = false;
    this.disremoveconsumption = false;
  }
  getadjustedvolumn(i: any, j: any, duration: string) {
    let postobj: object;
    let frmgrp: FormGroup
    frmgrp = (<FormGroup>((<FormArray>(
      (<FormGroup>(
        (<FormArray>this.datausage1.controls["_datausage"])
          .controls[i]
      )).controls["value"]
    )).controls[j]))
    postobj = {
      Id: frmgrp.get(duration + "-id").value,
      protestid: (<FormGroup>(
        (<FormArray>this.datausage1.controls["_datausage"])
          .controls[i]
      )).get('productid').value,
      type: frmgrp.get("column1").value,
      value: frmgrp.get(duration).value,
    }
    this._APIwithActionService.putAPI(this.forecastid, postobj, "Consumption", "GetAdjustedVolume").subscribe((data) => {
      this.makeformarray(JSON.parse(data["_body"]));
    })
  }
  removeconsumption() {
    let msg;
    this._APIwithActionService.getDatabyID(this.forecastsiteproductid, "Consumption", "Removedatausagefromsite", "param=" + this.forecastid).subscribe((data) => {
      msg = data.removedatausage
      if (msg != "") {
        this._GlobalAPIService.FailureMessage(msg);
      }
      else {
        this._APIwithActionService.getDatabyID(this.forecastid, "Consumption", "Bindforecastsiteproduct", "siteorcatid=" + this.selectedsiteid).subscribe((data) => {
        this.makeformarray(data);

        })
      }

    })
  }

  deleteseletedsitedatausage(id: any) {
    let msg;
    this._APIwithActionService.deleteData(this.forecastid, "Consumption", "removestefromdatausage", "siteid=" + id).subscribe((data) => {
      if (data["_body"] != "") {
        this.controlArray = [];
        let ss = (<FormArray>this.datausage1.controls["_datausage"]);
       ss.controls=[];
        let siteindex = this.Selectedsite.findIndex(x => x.siteID == id);
        if (siteindex >= 0) {
          this.Selectedsite.splice(siteindex, 1);
        }
        this.makearry=false;
      }
      else
      {
        let siteindex = this.Selectedsite.findIndex(x => x.siteID == id);
        if (siteindex >= 0) {
          this.Selectedsite.splice(siteindex, 1);
        }
      }
    })
    alert(id);
  }

  makeformarray(data:any)
  {
    this.controlArray = data.controls;
    this.colspan = this.controlArray.length + 1;
    this.datalist = data.datausage;
  

    let ss = (<FormArray>this.datausage1.controls["_datausage"]);
    ss.controls = [];
    for (let boxIndex = 0; boxIndex < this.datalist.length; boxIndex++) {
      this.adddatausge();
      (<FormGroup>(
        (<FormArray>this.datausage1.controls["_datausage"]).controls[
        boxIndex
        ]
      )).patchValue({
        productid: this.datalist[boxIndex].productid,
        productname: this.datalist[boxIndex].productname
      });
      let values: Array<any> = this.datalist[boxIndex].value;

      for (let packIndex = 0; packIndex < values.length; packIndex++) {


        (<FormArray>(
          (<FormGroup>(
            (<FormArray>this.datausage1.controls["_datausage"])
              .controls[boxIndex]
          )).controls["value"]
        )).push(this._fb.group(values[packIndex]))

      }


     
    }

  }
}
