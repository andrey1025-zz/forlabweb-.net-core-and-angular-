import { Component, OnInit, ElementRef, ViewChild, Renderer ,TemplateRef, Output, EventEmitter} from '@angular/core';
import {  ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GlobalVariable } from "../../shared/globalclass";
import { APIwithActionService } from "../../shared/APIwithAction.service";
import { GlobalAPIService } from "../../shared/GlobalAPI.service";
import { Servicestatisticdatausage1Component } from "../servicestatisticdatausage1/servicestatisticdatausage1.component";
import { Servicestatisticdatausage2Component } from "../servicestatisticdatausage2/servicestatisticdatausage2.component";
import { Servicestatisticdatausage3Component } from "../servicestatisticdatausage3/servicestatisticdatausage3.component"
import { ModalDirective } from "ngx-bootstrap";
import * as XLSX from 'xlsx';
import { ngxLoadingAnimationTypes, NgxLoadingComponent } from 'ngx-loading';
const PrimaryWhite = '#ffffff';
const SecondaryGrey = '#ccc';
const PrimaryRed = '#dd0031';
const SecondaryBlue = '#006ddd';
@Component({
  selector: 'app-servicestatisticadd',
  templateUrl: './servicestatisticadd.component.html',
 
})
export class ServicestatisticaddComponent implements OnInit {
  @ViewChild('ngxLoading') ngxLoadingComponent: NgxLoadingComponent;
  @ViewChild('customLoadingTemplate') customLoadingTemplate: TemplateRef<any>;
  public ngxLoadingAnimationTypes = ngxLoadingAnimationTypes;

  public primaryColour = PrimaryRed;
  public secondaryColour = SecondaryBlue;
  public coloursEnabled = false;
  public loadingTemplate: TemplateRef<any>;
  public config = { animationType: ngxLoadingAnimationTypes.none, primaryColour: this.primaryColour, secondaryColour: this.secondaryColour, tertiaryColour: this.primaryColour, backdropBorderRadius: '3px' };
  public loading=false
  importedlist = new Array();
  btncaption: string;
  checkstatus:boolean=false;
  Selectedtestid = new Array();
  Selectedsiteid = new Array();
  Sitelist=new Array();
  today = new Date();
  siteproductlist=new Array();
  validtxtscope: boolean = true;
  displayaattr: string = "none";
  serviceadd:FormGroup
  Period: string = "3";
  _month = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December")
  Quatermonthlist = new Array();
  _quarter = new Array("Qua1", "Qua2", "Qua3", "Qua4");
  Yearlist = new Array();
  ActualPeriod: string;
  public id: number = 0;
  forecastobject = new Object();
  TestAreaList = new Array();
  Testlist = new Array();
 

  ForecastID: string;
  startdate: string;
  methodology: string;
  reportingperiod: string;
  forecastperiod: string;
  datausage: string;
  
  wb: XLSX.WorkBook;
  Sheetarr = new Array();
  arrayBuffer: any;
  file: File;
  @Output()
  nextStep = new EventEmitter<string>();
  @ViewChild('general1') general1: ElementRef;
  @ViewChild('servicedata1') servicedata1: ElementRef;
  @ViewChild('lgModal') public lgModal: ModalDirective;//for add product
  @ViewChild('lgModal1') public lgModal1: ModalDirective;//for add period for actual consumption
  @ViewChild('lgModal2') public lgModal2: ModalDirective;//for add site
  @ViewChild('lgModal3') public lgModal3: ModalDirective;//for import
  @ViewChild('lgModal4') public lgModal4: ModalDirective;//for import
  @ViewChild(Servicestatisticdatausage1Component) datausage1: Servicestatisticdatausage1Component;
  @ViewChild(Servicestatisticdatausage2Component) datausage2: Servicestatisticdatausage2Component;
  @ViewChild(Servicestatisticdatausage3Component) datausage3: Servicestatisticdatausage3Component;
  constructor(private _fb: FormBuilder, private _rd: Renderer, private _GlobalAPIService: GlobalAPIService, private _avRoute: ActivatedRoute, private _APIwithActionService: APIwithActionService) {




    this.Bindlist()

    if (this._avRoute.snapshot.params["id"]) {
      this.id = this._avRoute.snapshot.params["id"];
    }
   
   }
 
   ngOnInit() {
    this.serviceadd = this._fb.group({
      ForecastID: 0,
      ForecastNo: ['', Validators.compose([Validators.required, Validators.maxLength(32)])],
      scopeofforecast: ['', [Validators.required]],
      Period: ['', [Validators.required]],
      Year: ['', [Validators.required]],
      datausage: 'DATA_USAGE1',
      txtscope: [''],
      QuartMonth: [''],
      extension: ['', [Validators.required]],
      txtdatausage: [{ value: GlobalVariable.Datausage1, disabled: true }],
      txt: [{ value: GlobalVariable.Servicedescription, disabled: true }],
      forecastdate: [{ value: new Date(), disabled: true }],
      LastUpdated: [{ value: new Date(), disabled: true }]
    }, {
        validator: this.txtscopevalidator.bind(this),

      })
    this.btncaption = "Save Forecast Info";


    if (this.id == 0) {
      // let consumptiondataele: HTMLElement = this.servicedata1.nativeElement as HTMLElement;
      // this._rd.setElementStyle(consumptiondataele, "display", "none")
    }
    else {
      this._APIwithActionService.getDatabyID(this.id, 'Forecsatinfo', 'GetbyId').subscribe((resp) => {
        console.log(resp)

        this.ForecastID = resp["forecastNo"];
        this.startdate = new Date(resp["startDate"]).getDate().toString() + "/" + new Date(resp["startDate"]).getMonth().toString() + "/" + new Date(resp["startDate"]).getFullYear().toString();
        this.methodology = "CONSUMPTION";
        this.reportingperiod = resp["period"];
        this.forecastperiod = resp["extension"];
        this.datausage = resp["dataUsage"];

        this.serviceadd.patchValue({
          ForecastID: this.id,
          ForecastNo: resp["forecastNo"],
          forecastdate: new Date(resp["forecastDate"]),
          LastUpdated: new Date(resp["lastUpdated"]),
          Year: new Date(resp["startDate"]).getFullYear().toString(),
          Period: resp["period"],
          extension: resp["extension"],
          scopeofforecast: resp["scopeOfTheForecast"] == "NATIONAL" || resp["scopeOfTheForecast"] == "GLOBAL" ? resp["scopeOfTheForecast"] : 'CUSTOM',
          datausage: resp["dataUsage"]


        });
        if (resp["scopeOfTheForecast"] == "NATIONAL" || resp["scopeOfTheForecast"] == "GLOBAL") {
          this.displayaattr = "none";
        }
        else {
          this.displayaattr = "block";
          this.serviceadd.patchValue({
            txtscope: resp["scopeOfTheForecast"]
          })
        }
        //displayaattr
        this.serviceadd.get('Period').disable();
        this.serviceadd.get('datausage').disable();
        this.getmonthlist(resp["period"])
        let month: number;
        month = new Date(resp["startDate"]).getMonth() + 1;
        if (resp["period"] == "Bimonthly" || resp["period"] == "Monthly") {

          this.serviceadd.patchValue({

            QuartMonth: this._month[month - 1]
          })
        }
        else if (resp["period"] == "Quarterly") {
          let quater;
          if (month == 1)
            quater = "Qua1";
          else if (month == 4)
            quater = "Qua2";
          else if (month == 7)
            quater = "Qua3";
          else
            quater = "Qua4";

          this.serviceadd.patchValue({
            QuartMonth: quater
          })
        }
      });
      // let consumptiondataele: HTMLElement = this.servicedata1.nativeElement as HTMLElement;

      // this._rd.setElementStyle(consumptiondataele, "display", "block");
    }

  }
 
 
  Bindlist() {
    for (let index = 2000; index < this.today.getFullYear() + 15; index++) {
      this.Yearlist.push(index)

    }
  }
  txtscopevalidator(group: FormGroup) {
    if (group.value.scopeofforecast == 'CUSTOM' && group.value.txtscope == "") {
      this.validtxtscope = false
    }
    else {
      this.validtxtscope = true
    }
  }
  getmonthlist(args) {

    switch (args) {
      case ("Bimonthly"):
        this.Quatermonthlist = this._month;
        break;
      case ("Monthly"):
        this.Quatermonthlist = this._month;
        break;
      case "Quarterly":
        this.Quatermonthlist = this._quarter;
        break;
      case "Yearly":
        this.Quatermonthlist.splice(0, this.Quatermonthlist.length)
        this.serviceadd.patchValue({
          QuartMonth: [{ value: '', disabled: true }]
        })
        break;
      default:
        break;
    }

  }

  selecttest(ischecked: boolean, id: any) {
    if (ischecked == true) {
      this.Selectedtestid.push(id);
    }
    else {
      // this.Selectedproductid.findIndex(x=>x===id);
      this.Selectedtestid.splice(this.Selectedtestid.findIndex(x => x === id), 1);
    }
    console.log(this.Selectedtestid);
  }




  selectsite(ischecked: boolean, data: any) {
    if (ischecked == true) {
      this.Selectedsiteid.push(data);
    }
    else {
      // this.Selectedproductid.findIndex(x=>x===id);
      this.Selectedsiteid.splice(this.Selectedsiteid.findIndex(x => x.siteID === data.siteID), 1);
    }
    this.checkstatus=false;
    console.log(this.Selectedsiteid);
  }
  displaytextscope(args) {
    if (args.target.value == "CUSTOM") {
      this.displayaattr = "block";
    }
    else {
      this.displayaattr = "none";
      this.serviceadd.patchValue({
        txtscope: ''
      })
    }
  }
  
  changedatausges(args) {
    if (args.target.checked == true) {
      switch (args.target.value) {
        case "DATA_USAGE1":
          this.serviceadd.patchValue({
            txtdatausage: GlobalVariable.Datausage1
          })
          break;
        case "DATA_USAGE2":
          this.serviceadd.patchValue({
            txtdatausage: GlobalVariable.Datausage2
          })
          break;
        case "DATA_USAGE3":
          this.serviceadd.patchValue({
            txtdatausage: GlobalVariable.Datausage3
          })
          break;
        default:
          break;
      }
    }
  }


  save() {
    if (this.btncaption == "Save Forecast Info") {
      this.saveforecastinfo();
    }
    else {
      this._GlobalAPIService.SuccessMessage("Service Statistic Saved Successfully");
    }
  }
  saveforecastinfo() {
    let startdate;
    let index;
    let monthInPeriod;
    let scopeofforecast;
    if (this.serviceadd.controls['scopeofforecast'].value != 'CUSTOM') {
      scopeofforecast = this.serviceadd.controls['scopeofforecast'].value;

    }
    else {
      scopeofforecast = this.serviceadd.controls['txtscope'].value;
    }
    if (this.serviceadd.controls['Period'].value == "Bimonthly" || this.serviceadd.controls['Period'].value == "Monthly") {
      index = this._month.indexOf(this.serviceadd.controls['QuartMonth'].value)
      startdate = new Date(this.serviceadd.controls['Year'].value, index + 1, 1)
    }
    else if (this.serviceadd.controls['Period'].value == "Quarterly") {
      if (this.serviceadd.controls['QuartMonth'].value == "Qua1") {
        startdate = new Date(this.serviceadd.controls['Year'].value, 1, 1)
      }
      else if (this.serviceadd.controls['QuartMonth'].value == "Qua2") {
        startdate = new Date(this.serviceadd.controls['Year'].value, 4, 1)
      }
      else if (this.serviceadd.controls['QuartMonth'].value == "Qua3") {
        startdate = new Date(this.serviceadd.controls['Year'].value, 7, 1)
      }
      else {
        startdate = new Date(this.serviceadd.controls['Year'].value, 10, 1)
      }
    }
    this.forecastobject = {
      ForecastID: this.id,
      ForecastNo: this.serviceadd.controls['ForecastNo'].value,
      methodology: 'SERVICE_STATISTIC',
      dataUsage: this.serviceadd.controls['datausage'].value,
      period: this.serviceadd.getRawValue().Period,
      extension: this.serviceadd.controls['extension'].value,
      scopeOfTheForecast: scopeofforecast,
      forecastDate: this.serviceadd.getRawValue().forecastdate,
      lastUpdated: this.serviceadd.getRawValue().LastUpdated,
      slowMovingPeriod: this.serviceadd.getRawValue().Period,
      method: 'Linear',
      startDate: startdate,
      forecastType: 'S',
      Status: "OPEN",
      Countryid: localStorage.getItem("countryid")
    }

    this._APIwithActionService.postAPI(this.forecastobject, 'Forecsatinfo', 'saveforecastinfo')
      .subscribe((data) => {
        if (data["_body"] != 0) {

          this._GlobalAPIService.SuccessMessage("Forecast Info Saved Successfully");
          this.btncaption = "Save Consumption Data";
          this.id = data["_body"];
        
          this._APIwithActionService.getDatabyID(this.id, 'Forecsatinfo', 'GetbyId').subscribe((resp) => {
            console.log(resp)
            this.ForecastID = resp["forecastNo"];
            this.startdate = new Date(resp["startDate"]).getDate().toString() + "/" + new Date(resp["startDate"]).getMonth().toString() + "/" + new Date(resp["startDate"]).getFullYear().toString();
            this.methodology = "CONSUMPTION";
            this.reportingperiod = resp["period"];
            this.forecastperiod = resp["extension"];
            this.datausage = resp["dataUsage"];
          })


          if (this.serviceadd.controls['datausage'].value == "DATA_USAGE1") 
          {
           this.nextStep.emit('step21,N,'+this.id);
          }
          else if(this.serviceadd.controls['datausage'].value == "DATA_USAGE2")
          {
            this.nextStep.emit('step22,N,'+this.id);
       //     this._router.navigate(["Demographic/aggregrateforecast",this.programid,Id])
            
          }
          else
          {
            this.nextStep.emit('step23,N,'+this.id);
          }

        }
        else {
          this._GlobalAPIService.FailureMessage("Duplicate ForecastID");

        }



      })
  }
 

 
}
