import { Component, OnInit,Input,ViewChild,TemplateRef, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup,FormArray} from '@angular/forms';
import { APIwithActionService } from "../../shared/APIwithAction.service";
import { GlobalAPIService } from "../../shared/GlobalAPI.service";
import { ActivatedRoute } from '@angular/router';
import { ngxLoadingAnimationTypes, NgxLoadingComponent } from 'ngx-loading';
const PrimaryWhite = '#ffffff';
const SecondaryGrey = '#ccc';
const PrimaryRed = '#dd0031';
const SecondaryBlue = '#006ddd';
@Component({
  selector: 'app-servicestatisticdatausage2',
  templateUrl: './servicestatisticdatausage2.component.html',
  styles: ['.table tr.active td {   background-color:#123456 !important;    color: white;  }'],
})
export class Servicestatisticdatausage2Component implements OnInit {
  @ViewChild('ngxLoading') ngxLoadingComponent: NgxLoadingComponent;
  @ViewChild('customLoadingTemplate') customLoadingTemplate: TemplateRef<any>;
  public ngxLoadingAnimationTypes = ngxLoadingAnimationTypes;

  public primaryColour = PrimaryRed;
  public secondaryColour = SecondaryBlue;
  public coloursEnabled = false;
  public loadingTemplate: TemplateRef<any>;
  public config = { animationType: ngxLoadingAnimationTypes.none, primaryColour: this.primaryColour, secondaryColour: this.secondaryColour, tertiaryColour: this.primaryColour, backdropBorderRadius: '3px' };
  public loading = false;
  Reportedsites = new Array();
  selectedsiteid: number;
  selectedRow2: Number;
  forecastsiteproductid: number = 0;
  param: string = "";
  selectedRow: Number;
  datausage2: FormGroup;
  disablenonreportedsite: boolean = true;
  disaddproduct: boolean = true;
  disactualconsumption: boolean = true;
  disremoveconsumption: boolean = true;
  Isnonreportedsite: boolean = false;
  NonReportedsites = new Array();
  controlArray = new Array();
  datalist = new Array();
  colspan: number = 0;
  totalsiteno:number=0;
  totaltestno:number=0;
  @Input() model;
  @Input() model2;
  @Input() model1;
  @Input() model3;
  forecastid:number
  @Input() RecforecastID1;
  @Output()
  nextStep = new EventEmitter<string>();
  constructor(private _avRoute: ActivatedRoute, private _fb: FormBuilder, private _APIwithActionService: APIwithActionService, private _GlobalAPIService: GlobalAPIService) { 


    if (this._avRoute.snapshot.params["id"]) {
      this.forecastid = this._avRoute.snapshot.params["id"];
    }
    this._APIwithActionService.GetDataUsage.subscribe((data: any) => {
this.makeformarray(data);

    }

    )
    this._APIwithActionService.getDatabyID(this.forecastid, "Consumption", "Getforecastsite").subscribe((data) => {
      this.Reportedsites = data;
    })
    this._APIwithActionService.Getsitesfordatausage2.subscribe((data: any) => {
      if (this.Isnonreportedsite == false) {
        data.forEach(element => {
          this.Reportedsites.push(element);
        });

      }
      else {
        //Add non reported site using api

        data.forEach(element => {
          this.NonReportedsites.push(element);
          this.param = this.param + element.siteID + ','
        });
        this.param = this.param + this.selectedsiteid + ','
        this._APIwithActionService.getDatabyID(this.forecastid, "Consumption", "Addnonrportedsites", "param=" + this.param).subscribe((data) => {
this.param="";
        })

      }
  
    })

    this._APIwithActionService.getDatabyID(this.forecastid,"Consumption","Gettotalsiteandproduct").subscribe((data)=>{
     
      this.totalsiteno=data.totalsiteno;
      this.totaltestno=data.totalproductno
       this.datausage2.patchValue({
         totalsite:data.totalsiteno,
         totalproductcount:data.totalproductno
       }) 
 
     })
  }
  openconsumption() {
    this.model1.show()
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
    this.datausage2 = this._fb.group({

      // selectedsiteList: this._fb.array([]),
      activesite:[{value:'',disabled:true}],
      totalsite:[{value:'',disabled:true}],
      productcount:[{value:'',disabled:true}],
      totalproductcount:[{value:'',disabled:true}],
      _datausage: this._fb.array([])




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
  Enableconsumptionbtn(i: any, j: any, duration: string) {
    let aa = (<FormGroup>((<FormArray>(
      (<FormGroup>(
        (<FormArray>this.datausage2.controls["_datausage"])
          .controls[i]
      )).controls["value"]
    )).controls[j])).controls[duration + "-id"].value
    console.log(aa);
   this.forecastsiteproductid = aa;
    this.disactualconsumption = false;
    this.disremoveconsumption = false;
  }



  getadjustedvolumn(i: any, j: any, duration: string) {
    let postobj: object;
    let frmgrp: FormGroup
    frmgrp = (<FormGroup>((<FormArray>(
      (<FormGroup>(
        (<FormArray>this.datausage2.controls["_datausage"])
          .controls[i]
      )).controls["value"]
    )).controls[j]))
    postobj = {
      Id: frmgrp.get(duration + "-id").value,
      protestid: (<FormGroup>(
        (<FormArray>this.datausage2.controls["_datausage"])
          .controls[i]
      )).get('productid').value,
      type: frmgrp.get("column1").value,
      value: frmgrp.get(duration).value,
    }
    this._APIwithActionService.putAPI(this.forecastid, postobj, "Consumption", "GetAdjustedVolume").subscribe((data) => {
      this.makeformarray(JSON.parse(data["_body"]));
    })
  }

  makeformarray(data:any)
  {
    this.controlArray = data.controls;
    this.colspan = this.controlArray.length + 1;
    this.datalist = data.datausage;
    console.log(data.datausage);

    let ss = (<FormArray>this.datausage2.controls["_datausage"]);
    ss.controls = [];
    for (let boxIndex = 0; boxIndex < this.datalist.length; boxIndex++) {
      this.adddatausge();
      (<FormGroup>(
        (<FormArray>this.datausage2.controls["_datausage"]).controls[
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
            (<FormArray>this.datausage2.controls["_datausage"])
              .controls[boxIndex]
          )).controls["value"]
        )).push(this._fb.group(values[packIndex]))

      }


      console.log((<FormArray>this.datausage2.controls["_datausage"]));
    }

  }

  initdatausge() {
    // Initialize add Box form field
    let boxForm: FormGroup = this._fb.group({
      productid:0,
      productname: [{ value: '', disabled: true }],
      value: this._fb.array([])
    });
    return boxForm;

  }

  adddatausge() {

    (<FormArray>this.datausage2.controls["_datausage"]).push(
      this.initdatausge()
    );
    // get box length for box name like box 1,box 2 in sidebar boxes combo list
    let connection_boxes_length = (<FormArray>(
      this.datausage2.controls["_datausage"]
    )).length;

  }
  openmodel() {
    this.Isnonreportedsite = false;
    this.model2.show();
  }
  openmodelfornonreported() {
    this.Isnonreportedsite = true;
    this.model2.show();
  }
  openmodel2() {
    this.model.show();
  }

  selectnonsitefordatausage2(data1, index) {
    this.selectedRow2 = index;
    this.selectedsiteid = data1.siteID;
   
  
    if (this.selectedsiteid != 0) {

      this._APIwithActionService.getDatabyID(this.forecastid, "Consumption", "Bindforecastsiteproduct", "siteorcatid=" + this.selectedsiteid).subscribe((data) => {
        this.datausage2.patchValue({
          activesite:data1.siteName,
          productcount:data.datausage.length
        })
 this.makeformarray(data);




      })
      this.disaddproduct = false;
      this.disablenonreportedsite = false;
    }
  }

  selectsitefordatausage2(data1, index) {
    this.selectedRow = index;
    this.selectedsiteid = data1.siteID;

   
    if (this.selectedsiteid != 0) {
      this._APIwithActionService.getDatabyID(this.forecastid, "Consumption", "Getforecastnonreportedsite", "siteid=" + this.selectedsiteid).subscribe((data) => {

        this.NonReportedsites = data;
       
      });
this.loading=true;
      this._APIwithActionService.getDatabyID(this.forecastid, "Consumption", "Bindforecastsiteproduct", "siteorcatid=" + this.selectedsiteid).subscribe((data) => {
       
        this.datausage2.patchValue({
          activesite:data1.siteName,
          productcount:data.datausage.length
        })
this.makeformarray(data);


this.loading=false;

      })
      this.disaddproduct = false;
      this.disablenonreportedsite = false;
    }
  }

  deleteseletedsitedatausage2(item, type) {

    let siteindex;
    if (type == 's' && this.NonReportedsites.length > 0) {
      this._GlobalAPIService.FailureMessage("First remove all non reportedsites ")
      return;
    }
    this._APIwithActionService.deleteData(this.forecastid, "Consumption", "removestefromdatausage", "siteid=" + item.siteID).subscribe((data) => {
      if (data["_body"] != "") {
        this.controlArray = [];
        let ss = (<FormArray>this.datausage2.controls["_datausage"]);
        ss.controls = [];
       
      }
      if (type == 's') {
        siteindex = this.Reportedsites.findIndex(x => x.siteID == item.siteID);
        if (siteindex >= 0) {
          this.Reportedsites.splice(siteindex, 1);
          this._GlobalAPIService.SuccessMessage(item.siteName+" Reported Site deleted Successfully");
        }
      }
      else {
        siteindex = this.NonReportedsites.findIndex(x => x.siteID == item.siteID);
        if (siteindex >= 0) {
          this.NonReportedsites.splice(siteindex, 1);
          this._GlobalAPIService.SuccessMessage(item.siteName+" Non Reported Site deleted Successfully");
        }
      }
    })

  }
}
