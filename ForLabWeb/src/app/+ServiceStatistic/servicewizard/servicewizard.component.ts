import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { ModalDirective } from "ngx-bootstrap";
import * as XLSX from 'xlsx';
import { Servicestatisticdatausage1Component } from '../servicestatisticdatausage1/servicestatisticdatausage1.component';

import { Servicestatisticdatausage2Component } from '../servicestatisticdatausage2/servicestatisticdatausage2.component';

import { Servicestatisticdatausage3Component } from '../servicestatisticdatausage3/servicestatisticdatausage3.component';

import { ngxLoadingAnimationTypes, NgxLoadingComponent } from 'ngx-loading';
import { ConductForecastComponent } from 'app/+ConductForecast/ConductForecast.component';

const PrimaryWhite = '#ffffff';
const SecondaryGrey = '#ccc';
const PrimaryRed = '#dd0031';
const SecondaryBlue = '#006ddd';

@Component({
  selector: 'app-servicewizard',
  templateUrl: './servicewizard.component.html',
  styleUrls: ['./servicewizard.component.css']
})
export class ServicewizardComponent implements OnInit {
  public id: number = 0;
  @ViewChild('lgModal') public lgModal: ModalDirective;//for add product
  @ViewChild('lgModal1') public lgModal1: ModalDirective;//for add period for actual consumption
  @ViewChild('lgModal2') public lgModal2: ModalDirective;//for add site
  @ViewChild('lgModal3') public lgModal3: ModalDirective;//for import
  @ViewChild('lgModal4') public lgModal4: ModalDirective;//for import
  @ViewChild(Servicestatisticdatausage1Component) datausage1: Servicestatisticdatausage1Component;
  @ViewChild(Servicestatisticdatausage2Component) datausage2: Servicestatisticdatausage2Component;
  @ViewChild(Servicestatisticdatausage3Component) datausage3: Servicestatisticdatausage3Component;
  @ViewChild(ConductForecastComponent) Conductforecast: ConductForecastComponent;
  @ViewChild('ngxLoading') ngxLoadingComponent: NgxLoadingComponent;
  @ViewChild('customLoadingTemplate') customLoadingTemplate: TemplateRef<any>;
  public ngxLoadingAnimationTypes = ngxLoadingAnimationTypes;

  public primaryColour = PrimaryRed;
  public secondaryColour = SecondaryBlue;
  public coloursEnabled = false;
  public loadingTemplate: TemplateRef<any>;
  public config = { animationType: ngxLoadingAnimationTypes.none, primaryColour: this.primaryColour, secondaryColour: this.secondaryColour, tertiaryColour: this.primaryColour, backdropBorderRadius: '3px' };
  forecastid: number;
  TestAreaList = new Array();
  Sitelist = new Array();
  Testlist = new Array();
  stringarr = new Array();
  Selectedtestid = new Array();
  Selectedsiteid = new Array();
  Period: string = "3";
  checkstatus: boolean = false;
  ActualPeriod: string;
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
  public loading = false
  Forecasttype: string;
  importedlist = new Array();
  constructor(private _fb: FormBuilder, private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService) {

    if (this._avRoute.snapshot.params["id"]) {
      this.id = this._avRoute.snapshot.params["id"];
      this.forecastid = this.id;
    }
    this._GlobalAPIService.getDataList('TestArea').subscribe((data) => {
      this.TestAreaList = data.aaData
      // console.log(this.ProductTypeList)
    });
    this._APIwithActionService.getDatabyID(localStorage.getItem("countryid"), 'Site', 'GetAll').subscribe((data) => {
      this.Sitelist = data.aaData;
    })
  }
  gettestlist(args) {
    this._APIwithActionService.getDatabyID(args.target.value, 'Test', 'GetAllTestsByAreaId').subscribe((data) => {
      this.Testlist = data



      // console.log(this.Instrumentlist)
    });
  }
  public steps = [
    {
      key: 'step1',
      title: 'General',
      valid: false,
      checked: false,
      submitted: false,
    },
    {
      key: 'step2',
      title: 'Service Data',
      valid: false,
      checked: false,
      submitted: false,
    },

    {
      key: 'step3',
      title: 'Conduct Forecast',
      valid: true,
      checked: false,
      submitted: false,
    }
  ];

  public activeStep = this.steps[0];
  ngOnInit() {
  }
  clearimportedconsumption() {
    this.importedlist = [];
  }
  saveimportedconsumption() {
    let Reportobject: Object;


    Reportobject = {
      receivereportdata: this.importedlist
    }
    this.loading = true;
    this._APIwithActionService.postAPI(Reportobject, 'Import', 'saveimportservice').subscribe((data: any) => {


      this.lgModal3.hide();
      this._GlobalAPIService.SuccessMessage(data["_body"]);
      this.loading = false;
      // this.importedlist = JSON.parse(data["_body"]);
    })


  }
  addactualconsumprion() {
    let params = "";
    let forecastsiteproductid;
    params = params + this.id + ','
    params = params + this.ActualPeriod + ','
    if (this.stringarr[0] == "step21") {
      forecastsiteproductid = this.datausage1.forecastsiteproductid
    }
    else if (this.stringarr[0] == "step23") {
      forecastsiteproductid = this.datausage3.forecastsiteproductid
    }
    else {
      forecastsiteproductid = this.datausage2.forecastsiteproductid
    }
    this._APIwithActionService.getDatabyID(forecastsiteproductid, 'Consumption', 'Addactualconsumption', 'param=' + params).subscribe((data) => {
      this._APIwithActionService.GetDataUsage.emit(data)
    })
    this.lgModal1.hide();

  }
  select() {

    let params = "";
    this.Selectedtestid.forEach(element => {
      params = params + element + ','
    });

    //second last element is site id
    if (this.stringarr[0] == "step21") {
      params = params + this.datausage1.selectedsiteid + ','
    }
    else if (this.stringarr[0] == "step23") {

      params = params + this.datausage3.selectedcategoryid + ','
    }
    else {
      this.datausage2.totaltestno = this.datausage2.totaltestno + this.Selectedtestid.length

      this.datausage2.datausage2.patchValue({
        totalproductcount: this.datausage2.totaltestno
      });
      params = params + this.datausage2.selectedsiteid + ','
    }
    // last element is noofperiod
    params = params + this.Period
    this._APIwithActionService.getDatabyID(this.id, 'Consumption', 'GetDataUasge', 'param=' + params).subscribe((data) => {



      this._APIwithActionService.GetDataUsage.emit(data)
      this.Selectedtestid.splice(0, this.Selectedtestid.length);

    })
    this.lgModal.hide();
  }
  selecttest(ischecked: boolean, id: any) {
    if (ischecked == true) {
      this.Selectedtestid.push(id);
    }
    else {
      // this.Selectedproductid.findIndex(x=>x===id);
      this.Selectedtestid.splice(this.Selectedtestid.findIndex(x => x === id), 1);
    }

  }




  selectsite(ischecked: boolean, data: any) {
    if (ischecked == true) {
      this.Selectedsiteid.push(data);
    }
    else {
      // this.Selectedproductid.findIndex(x=>x===id);
      this.Selectedsiteid.splice(this.Selectedsiteid.findIndex(x => x.siteID === data.siteID), 1);
    }
    this.checkstatus = false;
    console.log(this.Selectedsiteid);
  }

  getselectedsite() {
    this._APIwithActionService.Getsitesfordatausage2.emit(this.Selectedsiteid);
    // this.Selectedsiteid.forEach
    this.Selectedsiteid.forEach(element => {
      this.Sitelist.splice(this.Sitelist.findIndex(x => x == element), 1)
    });
    if (this.stringarr[0] == "step22") {
      this.datausage2.totalsiteno = this.datausage2.totalsiteno + this.Selectedsiteid.length;
      this.datausage2.datausage2.patchValue({
        totalsite: this.datausage2.totalsiteno
      });
    }
    this.Selectedsiteid = [];
    this.lgModal2.hide();
  }

  incomingfile(event) {
    this.file = event.target.files[0];
    const target: DataTransfer = <DataTransfer>(event.target);
    if (target.files.length !== 1) throw new Error('Cannot use multiple files');
    const reader: FileReader = new FileReader();
    reader.onload = (e: any) => {
      /* read workbook */
      const bstr: string = e.target.result;
      this.wb = XLSX.read(bstr, { type: 'binary' });
      /* grab first sheet */
      this.Sheetarr = this.wb.SheetNames;
      const wsname: string = this.wb.SheetNames[0];
      const ws: XLSX.WorkSheet = this.wb.Sheets["Hist consumption corrected"];
      /* save data */
      var data = XLSX.utils.sheet_to_json(ws, { header: 1 });
    };
    reader.readAsBinaryString(target.files[0]);
    this.lgModal4.show();
  }
  SelectSheet(sheetname) {
    this.lgModal4.hide();
    const ws: XLSX.WorkSheet = this.wb.Sheets[sheetname];
    /* save data */
    var data1 = XLSX.utils.sheet_to_json(ws, { header: 1 });
    console.log(data1)
    let arr = new Array()
    for (let index = 1; index < data1.length; index++) {
      arr.push(data1[index])
    }
    this.loading = true;
    this._APIwithActionService.putAPI(this.id, arr, 'Import', 'importservice').subscribe((data: any) => {
      console.log(JSON.parse(data["_body"]));
      this.importedlist = JSON.parse(data["_body"]);
      this.loading = false;
    })

  }
  nextStep(event) {

    // let 
    this.stringarr = event.split(',')
    // this.forecastid=this.stringarr[1]
    // console.log( this.forecastid)

    this.id = this.stringarr[2]

    if (this.stringarr[1] == 'N') {
      if (this.stringarr[0] == "step21") {
        this.Forecasttype = "datausage1";
        // this.datausage1.forecastid= this.id1
        this.activeStep = this.steps[1]
      }
      else if (this.stringarr[0] == "step22") {
        this.Forecasttype = "datausage2";
        this.activeStep = this.steps[1]

      }
      else if (this.stringarr[0] == "step23") {
        this.Forecasttype = "datausage3";


        // this.datausage3.forecastid=  this.id ;
        this.activeStep = this.steps[1]

      }
      else if (this.stringarr[0] == "step3") {
        this.Conductforecast.ngOnInit();
        this.activeStep = this.steps[2]

      }
    }
    else {
      if (this.stringarr[0] == "step1") {
        //  this.Forecasttype = "datausage1";
        this.activeStep = this.steps[0]
      }
      else if (this.stringarr[0] == "step22") {
        this.Forecasttype = "datausage2";
        this.activeStep = this.steps[1]

      }
      else if (this.stringarr[0] == "step23") {
        this.Forecasttype = "datausage3";


        // this.datausage3.forecastid=  this.id ;
        this.activeStep = this.steps[1]

      }
      else if (this.stringarr[0] == "step21") {
        this.Forecasttype = "datausage1";


        // this.datausage3.forecastid=  this.id ;
        this.activeStep = this.steps[1]

      }
    }


  }
}
