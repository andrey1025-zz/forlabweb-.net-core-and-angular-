import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { ModalDirective } from "ngx-bootstrap";
import * as XLSX from 'xlsx';
import { Consumptiondatausage1Component } from '../consumptiondatausage1/consumptiondatausage1.component';

import { Consumptiondatausage2Component } from '../consumptiondatausage2/consumptiondatausage2.component';

import { Consumptiondatausage3Component } from '../consumptiondatausage3/consumptiondatausage3.component';

import { ngxLoadingAnimationTypes, NgxLoadingComponent } from 'ngx-loading';
import { ConductForecastComponent } from 'app/+ConductForecast/ConductForecast.component';

const PrimaryWhite = '#ffffff';
const SecondaryGrey = '#ccc';
const PrimaryRed = '#dd0031';
const SecondaryBlue = '#006ddd';
@Component({
  selector: 'app-consumptionwizard',
  templateUrl: './consumptionwizard.component.html',
  styleUrls: ['./consumptionwizard.component.css']
})
export class ConsumptionwizardComponent implements OnInit {
  ActualPeriod: string;
  importedlist = new Array();
  public loading = false;
  //demosettingadd: FormGroup;
  FormTitle: string = "";
  Foracstinfoobj: Object;
  classname: string = "";
  programid: number = 0;
  forecastid: number = 0;
  type: string;
  date: Date;
  RegionList = new Array();
  Productlist = new Array();
  Sitelist = new Array();
  Forecasttype: string = "A";
  static forecastid1;
  stringarr = new Array();
  ProductTypeList = new Array();
  public id: number = 0;
  public id1: number = 0;
  Period: string = "3";
  Selectedproductid = new Array();
  ForecastID: string;
  startdate: string;
  methodology: string;
  reportingperiod: string;
  forecastperiod: string;
  datausage: string;
  wb: XLSX.WorkBook;
  Sheetarr = new Array();
  Selectedsiteid = new Array();
  checkstatus: boolean = false;
  arrayBuffer: any;
  file: File;
  @ViewChild('lgModal') public lgModal: ModalDirective;//for add product
  @ViewChild('lgModal1') public lgModal1: ModalDirective;//for add period for actual consumption
  @ViewChild('lgModal2') public lgModal2: ModalDirective;//for add site
  @ViewChild('lgModal3') public lgModal3: ModalDirective;//for import
  @ViewChild('lgModal4') public lgModal4: ModalDirective;//for import
  @ViewChild(Consumptiondatausage1Component) datausage1: Consumptiondatausage1Component;
  @ViewChild(Consumptiondatausage2Component) datausage2: Consumptiondatausage2Component;
  @ViewChild(Consumptiondatausage3Component) datausage3: Consumptiondatausage3Component;
  @ViewChild(ConductForecastComponent) Conductforecast: ConductForecastComponent;
  @ViewChild('ngxLoading') ngxLoadingComponent: NgxLoadingComponent;
  @ViewChild('customLoadingTemplate') customLoadingTemplate: TemplateRef<any>;
  public ngxLoadingAnimationTypes = ngxLoadingAnimationTypes;

  public primaryColour = PrimaryRed;
  public secondaryColour = SecondaryBlue;
  public coloursEnabled = false;
  public loadingTemplate: TemplateRef<any>;
  public config = { animationType: ngxLoadingAnimationTypes.none, primaryColour: this.primaryColour, secondaryColour: this.secondaryColour, tertiaryColour: this.primaryColour, backdropBorderRadius: '3px' };

  // @ViewChild(TestingprotocolComponent) Testingprotocol: TestingprotocolComponent;
  // @ViewChild(PatientAssumptionComponent) PatientAssumption: PatientAssumptionComponent;
  // @ViewChild(ProductassumptionComponent) Productassumption: ProductassumptionComponent;
  // @ViewChild(LineargrowthComponent) lineargrowth: LineargrowthComponent;
  // @ViewChild(ForecastchartComponent) forecastchart: ForecastchartComponent;
  // @ViewChild(AggregrateforecastComponent) Aggregate: AggregrateforecastComponent;
  constructor(private _fb: FormBuilder, private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService) {
    if (this._avRoute.snapshot.params["id"]) {
      this.id = this._avRoute.snapshot.params["id"];
      this.forecastid = this.id;
    }

    this._GlobalAPIService.getDataList('ProductType').subscribe((data) => {
      this.ProductTypeList = data.aaData

    });
    this._APIwithActionService.getDatabyID(localStorage.getItem("countryid"), 'Site', 'GetAll').subscribe((data) => {
      this.Sitelist = data.aaData;
    })

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
      title: 'Consumption Data',
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
    if (this.type == "C") {
      this.activeStep = this.steps[7];
    }

  }

  selectproduct(ischecked: boolean, id: any) {
    if (ischecked == true) {
      this.Selectedproductid.push(id);
    }
    else {
      // this.Selectedproductid.findIndex(x=>x===id);
      this.Selectedproductid.splice(this.Selectedproductid.findIndex(x => x === id), 1);
    }

  }

  setActiveStep(steo) {
    this.activeStep = steo
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

  }
  prevStep() {
    let idx = this.steps.indexOf(this.activeStep);
    if (idx > 0) {
      this.activeStep = this.steps[idx - 1]
    }
  }

  nextStep(event) {

    // let 
    this.stringarr = event.split(',')
    // this.forecastid=this.stringarr[1]

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

  getproductlist(args) {
    this._APIwithActionService.getDatabyID(args.target.value, 'Product', 'GetAllProductByType').subscribe((data) => {
      this.Productlist = data




    });
  }
  onWizardComplete(data) {

  }


  private lastModel;

  // custom change detection
  backtotabs(event) {
    alert('jj');
    if (event == "step2S") {
      this.Forecasttype = "S";
      this.activeStep = this.steps[1]
    }
    else if (event == "step2A") {
      this.Forecasttype = "A";
      this.activeStep = this.steps[1]
    }
    else if (event == "step1") {
      this.activeStep = this.steps[0]
    }
    else if (event == "step3") {
      this.activeStep = this.steps[2]
    }
    else if (event == "step4") {
      this.activeStep = this.steps[3]
    }
    else if (event == "step5") {
      this.activeStep = this.steps[4]
    }
    else if (event == "step6") {
      this.activeStep = this.steps[5]
    }
    else if (event == "step7") {
      this.activeStep = this.steps[6]
    }
    else if (event == "step8") {
      this.activeStep = this.steps[7]
    }




    // if (this.CurrentTab == 7) {

    //     let el: HTMLElement = this.testingprotocol1.nativeElement as HTMLElement;
    //     el.click();

  }

  getselectedsite() {
    this._APIwithActionService.Getsitesfordatausage2.emit(this.Selectedsiteid);
    // this.Selectedsiteid.forEach
    this.Selectedsiteid.forEach(element => {
      this.Sitelist.splice(this.Sitelist.findIndex(x => x == element), 1)
    });
    //second last element is site id
    if (this.stringarr[0] == "step22") {
      this.datausage2.totalsiteno = this.datausage2.totalsiteno + this.Selectedsiteid.length;
      this.datausage2.datausage2.patchValue({
        totalsite: this.datausage2.totalsiteno
      });
    }
    this.Selectedsiteid = [];
    this.lgModal2.hide();
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
    this.Selectedproductid.forEach(element => {
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
      params = params + this.datausage2.selectedsiteid + ','
      this.datausage2.totalproductno = this.datausage2.totalproductno + this.Selectedproductid.length

      this.datausage2.datausage2.patchValue({
        totalproductcount: this.datausage2.totalproductno
      });
    }
    // last element is noofperiod
    params = params + this.Period
    this._APIwithActionService.getDatabyID(this.id, 'Consumption', 'GetDataUasge', 'param=' + params).subscribe((data) => {



      this._APIwithActionService.GetDataUsage.emit(data)
      this.Selectedproductid.splice(0, this.Selectedproductid.length);

    })
    this.lgModal.hide();
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

    let arr = new Array()


    for (let index = 1; index < data1.length; index++) {

      arr.push(data1[index])

    }
    this.loading = true;
    //  this.lgModal6.show();
    this._APIwithActionService.putAPI(this.id, arr, 'Import', 'importconsumption').subscribe((data: any) => {


      this.importedlist = JSON.parse(data["_body"]);
      this.loading = false;
      //  this.lgModal6.hide();
    })

  }

  importconsumption() {
    let fileReader = new FileReader();
    fileReader.readAsDataURL(this.file);
    fileReader.onload = (e) => {
      this.arrayBuffer = fileReader.result;
      var data = new Uint8Array(this.arrayBuffer);
      var arr = new Array();
      for (var i = 0; i != data.length; ++i) arr[i] = String.fromCharCode(data[i]);
      var bstr = arr.join("");
      var workbook = XLSX.read(bstr, { type: "binary" });

      // var first_sheet_name = workbook.SheetNames[1];
      var worksheet = workbook.Sheets["Site"];

    }
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
    this._APIwithActionService.postAPI(Reportobject, 'Import', 'saveimportconsumption').subscribe((data: any) => {

      this.lgModal3.hide();
      this._GlobalAPIService.SuccessMessage(data["_body"]);

      this.loading = false;
      // this.importedlist = JSON.parse(data["_body"]);
    })
  }
}
