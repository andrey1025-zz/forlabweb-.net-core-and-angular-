import { Component, OnInit, ElementRef, ViewChild, Renderer,TemplateRef, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GlobalVariable } from "../../shared/globalclass";
import { APIwithActionService } from "../../shared/APIwithAction.service";
import { GlobalAPIService } from "../../shared/GlobalAPI.service";
import { Consumptiondatausage1Component } from "../consumptiondatausage1/consumptiondatausage1.component";
import { Consumptiondatausage2Component } from "../consumptiondatausage2/consumptiondatausage2.component";
import { Consumptiondatausage3Component } from "../consumptiondatausage3/consumptiondatausage3.component";
import { ModalDirective } from "ngx-bootstrap";
import * as XLSX from 'xlsx';
import { ngxLoadingAnimationTypes, NgxLoadingComponent } from 'ngx-loading';

const PrimaryWhite = '#ffffff';
const SecondaryGrey = '#ccc';
const PrimaryRed = '#dd0031';
const SecondaryBlue = '#006ddd';
@Component({
  selector: 'app-consumption-add',
  templateUrl: './consumption-add.component.html'
})
export class ConsumptionAddComponent implements OnInit {
  importedlist = new Array();
  public loading = false;
  btncaption: string;
  checkstatus: boolean = false;
  Selectedproductid = new Array();
  Selectedsiteid = new Array();
  Sitelist = new Array();
  today = new Date();
  siteproductlist = new Array();
  validtxtscope: boolean = true;
  displayaattr: string = "none";
  consumptionadd: FormGroup;
  Period: string = "3";
  ForecastID: string;
  startdate: string;
  methodology: string;
  reportingperiod: string;
  forecastperiod: string;
  datausage: string;
  wb: XLSX.WorkBook;
  Sheetarr = new Array();
  _month = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December")
  Quatermonthlist = new Array();
  _quarter = new Array("Qua1", "Qua2", "Qua3", "Qua4");
  Yearlist = new Array();
  ActualPeriod: string;
  public id: number = 0;
  forecastobject = new Object();
  ProductTypeList = new Array();
  Productlist = new Array();
  arrayBuffer: any;
  file: File;
  @Output()
  nextStep = new EventEmitter<string>();
  @ViewChild('ngxLoading') ngxLoadingComponent: NgxLoadingComponent;
  @ViewChild('customLoadingTemplate') customLoadingTemplate: TemplateRef<any>;
  public ngxLoadingAnimationTypes = ngxLoadingAnimationTypes;

  public primaryColour = PrimaryRed;
  public secondaryColour = SecondaryBlue;
  public coloursEnabled = false;
  public loadingTemplate: TemplateRef<any>;
  public config = { animationType: ngxLoadingAnimationTypes.none, primaryColour: this.primaryColour, secondaryColour: this.secondaryColour, tertiaryColour: this.primaryColour, backdropBorderRadius: '3px' };
  @ViewChild('general1') general1: ElementRef;
  @ViewChild('consumptiondata1') consumptiondata1: ElementRef;
  @ViewChild('lgModal') public lgModal: ModalDirective;//for add product
  @ViewChild('lgModal1') public lgModal1: ModalDirective;//for add period for actual consumption
  @ViewChild('lgModal2') public lgModal2: ModalDirective;//for add site
  @ViewChild('lgModal3') public lgModal3: ModalDirective;//for import
  @ViewChild('lgModal4') public lgModal4: ModalDirective;//for import
  //@ViewChild('lgModal6') public lgModal6: ModalDirective;//for import
  @ViewChild(Consumptiondatausage1Component) datausage1: Consumptiondatausage1Component;
  @ViewChild(Consumptiondatausage2Component) datausage2: Consumptiondatausage2Component;
  @ViewChild(Consumptiondatausage3Component) datausage3: Consumptiondatausage3Component;
  constructor(private _fb: FormBuilder, private _rd: Renderer, private _GlobalAPIService: GlobalAPIService, private _avRoute: ActivatedRoute, private _APIwithActionService: APIwithActionService) {
    this.Bindlist()

    if (this._avRoute.snapshot.params["id"]) {
      this.id = this._avRoute.snapshot.params["id"];
    }
    this._GlobalAPIService.getDataList('ProductType').subscribe((data) => {
      this.ProductTypeList = data.aaData
   
    });
    this._APIwithActionService.getDatabyID(localStorage.getItem("countryid"),'Site','GetAll').subscribe((data) => {
      this.Sitelist = data.aaData;
    })
  }

  getproductlist(args) {
    this._APIwithActionService.getDatabyID(args.target.value, 'Product', 'GetAllProductByType').subscribe((data) => {
      this.Productlist = data

    });
  }

  ngOnInit() {
    this.consumptionadd = this._fb.group({
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
      txt: [{ value: GlobalVariable.description, disabled: true }],
      forecastdate: [{ value: new Date(), disabled: true }],
      LastUpdated: [{ value: new Date(), disabled: true }]
    }, {
        validator: this.txtscopevalidator.bind(this),

      })
    this.btncaption = "Save Forecast Info";


    if (this.id == 0) {
      // let consumptiondataele: HTMLElement = this.consumptiondata1.nativeElement as HTMLElement;
      // this._rd.setElementStyle(consumptiondataele, "display", "none")
    }
    else {
      this._APIwithActionService.getDatabyID(this.id, 'Forecsatinfo', 'GetbyId').subscribe((resp) => {
     
        this.ForecastID = resp["forecastNo"];
        this.startdate = new Date(resp["startDate"]).getDate().toString() + "/" + new Date(resp["startDate"]).getMonth().toString() + "/" + new Date(resp["startDate"]).getFullYear().toString();
        this.methodology = "CONSUMPTION";
        this.reportingperiod = resp["period"];
        this.forecastperiod = resp["extension"];
        this.datausage = resp["dataUsage"];

        this.consumptionadd.patchValue({
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
          this.consumptionadd.patchValue({
            txtscope: resp["scopeOfTheForecast"]
          })
        }
        //displayaattr
        this.consumptionadd.get('Period').disable();
        this.consumptionadd.get('datausage').disable();
        this.getmonthlist(resp["period"])
        let month: number;
        month = new Date(resp["startDate"]).getMonth() + 1;
        if (resp["period"] == "Bimonthly" || resp["period"] == "Monthly") {

          this.consumptionadd.patchValue({

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

          this.consumptionadd.patchValue({
            QuartMonth: quater
          })
        }
      });
      // let consumptiondataele: HTMLElement = this.consumptiondata1.nativeElement as HTMLElement;

      // this._rd.setElementStyle(consumptiondataele, "display", "block");
    }
  
  }
  generalclick() {
    this.btncaption = "Save Forecast Info";
  }
  consumptiondataclick() {
    this.btncaption = "Save Consumption Data";

    //second last element is site id
    if (this.consumptionadd.controls['datausage'].value == "DATA_USAGE2") {
      this.datausage2.Reportedsites.forEach(element => {
        this.Sitelist.splice(this.Sitelist.findIndex(x => x == element), 1)
      }


      )
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
  selectproduct(ischecked: boolean, id: any) {
    if (ischecked == true) {
      this.Selectedproductid.push(id);
    }
    else {
      // this.Selectedproductid.findIndex(x=>x===id);
      this.Selectedproductid.splice(this.Selectedproductid.findIndex(x => x === id), 1);
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
   
  }
  displaytextscope(args) {
    if (args.target.value == "CUSTOM") {
      this.displayaattr = "block";
    }
    else {
      this.displayaattr = "none";
      this.consumptionadd.patchValue({
        txtscope: ''
      })
    }
  }
  Bindlist() {
    for (let index = 2000; index < this.today.getFullYear() + 15; index++) {
      this.Yearlist.push(index)

    }
  }
  changedatausges(args) {
    if (args.target.checked == true) {
      switch (args.target.value) {
        case "DATA_USAGE1":
          this.consumptionadd.patchValue({
            txtdatausage: GlobalVariable.Datausage1
          })
          break;
        case "DATA_USAGE2":
          this.consumptionadd.patchValue({
            txtdatausage: GlobalVariable.Datausage2
          })
          break;
        case "DATA_USAGE3":
          this.consumptionadd.patchValue({
            txtdatausage: GlobalVariable.Datausage3
          })
          break;
        default:
          break;
      }
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
        this.consumptionadd.patchValue({
          QuartMonth: [{ value: '', disabled: true }]
        })
        break;
      default:
        break;
    }

  }
  save() {
    if (this.btncaption == "Save Forecast Info") {
      this.saveforecastinfo();




    }
    else {
      this._GlobalAPIService.SuccessMessage("Consumption Saved Successfully");
    }
  }
  saveforecastinfo() {
    let startdate;
    let index;
    let monthInPeriod;
    let scopeofforecast;
    if (this.consumptionadd.controls['scopeofforecast'].value != 'CUSTOM') {
      scopeofforecast = this.consumptionadd.controls['scopeofforecast'].value;

    }
    else {
      scopeofforecast = this.consumptionadd.controls['txtscope'].value;
    }
    if (this.consumptionadd.controls['Period'].value == "Bimonthly" || this.consumptionadd.controls['Period'].value == "Monthly") {
      index = this._month.indexOf(this.consumptionadd.controls['QuartMonth'].value)
      startdate = new Date(this.consumptionadd.controls['Year'].value, index + 1, 1)
    }
    else if (this.consumptionadd.controls['Period'].value == "Quarterly") {
      if (this.consumptionadd.controls['QuartMonth'].value == "Qua1") {
        startdate = new Date(this.consumptionadd.controls['Year'].value, 1, 1)
      }
      else if (this.consumptionadd.controls['QuartMonth'].value == "Qua2") {
        startdate = new Date(this.consumptionadd.controls['Year'].value, 4, 1)
      }
      else if (this.consumptionadd.controls['QuartMonth'].value == "Qua3") {
        startdate = new Date(this.consumptionadd.controls['Year'].value, 7, 1)
      }
      else {
        startdate = new Date(this.consumptionadd.controls['Year'].value, 10, 1)
      }
    }
    
        
    this.forecastobject = {
      ForecastID: this.id,
      ForecastNo: this.consumptionadd.controls['ForecastNo'].value,
      methodology: 'CONSUMPTION',
      dataUsage: this.consumptionadd.controls['datausage'].value,
      period: this.consumptionadd.getRawValue().Period,
      extension: this.consumptionadd.controls['extension'].value,
      scopeOfTheForecast: scopeofforecast,
      forecastDate: this.consumptionadd.getRawValue().forecastdate,
      lastUpdated: this.consumptionadd.getRawValue().LastUpdated,
      slowMovingPeriod: this.consumptionadd.getRawValue().Period,
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
          //second last element is site id
          // if (this.consumptionadd.controls['datausage'].value == "DATA_USAGE1") {
          //   this.datausage1.forecastid = this.id;
          // }
          // else if (this.consumptionadd.controls['datausage'].value == "DATA_USAGE2") {

          //   this.datausage2.forecastid = this.id;
          // }
          // else {
          //   this.datausage3.forecastid = this.id
          // }
          // let consumptiondataele: HTMLElement = this.consumptiondata1.nativeElement as HTMLElement;

          //    this._rd.setElementStyle(consumptiondataele, "display", "block");
          // consumptiondataele.click();
          this._APIwithActionService.getDatabyID(this.id, 'Forecsatinfo', 'GetbyId').subscribe((resp) => {
          
            this.ForecastID = resp["forecastNo"];
            this.startdate = new Date(resp["startDate"]).getDate().toString() + "/" + new Date(resp["startDate"]).getMonth().toString() + "/" + new Date(resp["startDate"]).getFullYear().toString();
            this.methodology = "CONSUMPTION";
            this.reportingperiod = resp["period"];
            this.forecastperiod = resp["extension"];
            this.datausage = resp["dataUsage"];
          })
          if (this.consumptionadd.controls['datausage'].value == "DATA_USAGE1") 
          {
           this.nextStep.emit('step21,N,'+this.id);
          }
          else if(this.consumptionadd.controls['datausage'].value == "DATA_USAGE2")
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
  select() {

    let params = "";
    this.Selectedproductid.forEach(element => {
      params = params + element + ','
    });
  
    //second last element is site id
    if (this.consumptionadd.controls['datausage'].value == "DATA_USAGE1") {
      params = params + this.datausage1.selectedsiteid + ','
    }
    else if (this.consumptionadd.controls['datausage'].value == "DATA_USAGE3") {

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
  getselectedsite() {
    this._APIwithActionService.Getsitesfordatausage2.emit(this.Selectedsiteid);
    // this.Selectedsiteid.forEach
    this.Selectedsiteid.forEach(element => {
      this.Sitelist.splice(this.Sitelist.findIndex(x => x == element), 1)
    });
    //second last element is site id
    if (this.consumptionadd.controls['datausage'].value == "DATA_USAGE2") {
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
    if (this.consumptionadd.controls['datausage'].value == "DATA_USAGE1") {
      forecastsiteproductid = this.datausage1.forecastsiteproductid
    }
    else if (this.consumptionadd.controls['datausage'].value == "DATA_USAGE3") {
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
    this.loading=true;
  //  this.lgModal6.show();
    this._APIwithActionService.putAPI(this.id, arr, 'Import', 'importconsumption').subscribe((data: any) => {

    
      this.importedlist = JSON.parse(data["_body"]);
      this.loading=false;
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
  clearimportedconsumption()
  {
    this.importedlist=[];
  }
  saveimportedconsumption() {
    let Reportobject: Object;


    Reportobject={
      receivereportdata:this.importedlist
    }
    this.loading=true;
    this._APIwithActionService.postAPI(Reportobject, 'Import', 'saveimportconsumption').subscribe((data: any) => {

      this.lgModal3.hide();
      this._GlobalAPIService.SuccessMessage(data["_body"]);

      this.loading=false;
     // this.importedlist = JSON.parse(data["_body"]);
    })
  }
}
