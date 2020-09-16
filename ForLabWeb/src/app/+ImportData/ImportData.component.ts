import { Component, ViewChild, TemplateRef, Renderer, AfterViewInit, OnInit } from '@angular/core';
import * as XLSX from 'xlsx';
import { APIwithActionService } from "../shared/APIwithAction.service";
import { GlobalAPIService } from "../shared/GlobalAPI.service";
import { HttpClient, HttpRequest, HttpEventType, HttpResponse, HttpHeaders, HttpParams } from '@angular/common/http'
import { FormGroup, Validators, FormBuilder } from '@angular/forms';

import { ngxLoadingAnimationTypes, NgxLoadingComponent } from 'ngx-loading';
import { ModalDirective } from 'ngx-bootstrap';
import { element } from 'protractor';
import * as FileSaver from 'file-saver';
const PrimaryWhite = '#ffffff';
const SecondaryGrey = '#ccc';
const PrimaryRed = '#dd0031';
const SecondaryBlue = '#006ddd';
const fileType = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
const fileExtension = '.xlsx';
@Component({
  selector: 'sa-importdata',
  templateUrl: './ImportData.component.html'
})
export class ImportDataComponent implements OnInit {
  arrayBuffer: any;
  file: File;
  Importform: FormGroup;
  btntext: string = "Upload";
  Shoediv: boolean = false;
  public progress: number;
  public message: string = "";
  public messagearr = new Array();
  public Show: boolean = false;
  ProductTypeList = new Array();
  Testingarealist = new Array();
  Testingarealist1 = new Array();
  Sheetarr = new Array();
  public demo2: any;
  importoption: FormGroup;
  selectedsheet = new Array();
  wb: XLSX.WorkBook;
  @ViewChild('lgModal4') public lgModal4: ModalDirective;

  @ViewChild('ngxLoading') ngxLoadingComponent: NgxLoadingComponent;
  @ViewChild('customLoadingTemplate') customLoadingTemplate: TemplateRef<any>;
  public ngxLoadingAnimationTypes = ngxLoadingAnimationTypes;
  checkstatus: boolean;
  btnprocess: string = "Download";
  radiobuttonval: string = "";
  public primaryColour = PrimaryRed;
  public secondaryColour = SecondaryBlue;
  public coloursEnabled = false;
  public loadingTemplate: TemplateRef<any>;
  formData = new FormData();
  selectedarea = new Array();
  selectedproduct = new Array();
  public config = { animationType: ngxLoadingAnimationTypes.none, primaryColour: this.primaryColour, secondaryColour: this.secondaryColour, tertiaryColour: this.primaryColour, backdropBorderRadius: '3px' };
  public loading = false;
  checkedtestarea: boolean = true;
  checkedproducttype: boolean = true;
  checkedteststatus: boolean = true;
  checkedproductstatus: boolean = true;
  checkedinstrumentstatus: boolean = true;
  checkedsitestatus: boolean = true;
  checkedsiteinsstatus: boolean = true;
  checkedtestproductusagestatus: boolean = true;
  checkedconsumblesstatus: boolean = true;
  showclass: boolean = true;
  regionlist = new Array();
  sitelist = new Array();
  inslist = new Array();
  testlist = new Array();
  productlist = new Array();
  siteinstrument = new Array();
  testproductusage = new Array();
  consumbleslist = new Array();
  constructor(private Renderer: Renderer, private _fb: FormBuilder, private http: HttpClient, private _APIwithActionService: APIwithActionService, private _GlobalAPIService: GlobalAPIService) {
    this.Importform = this._fb.group({
      file: [null, Validators.required]
    });




    this.importoption = this._fb.group({

      downloadoption: 'b'



    })


  }
  ngOnInit() {
    this.getTestingArea();
  }
  checkedoption(event: any, type: string) {
    if (type == "Test") {
      if (event.target.checked == true) {
        this.checkedtestarea = true
        this.Testingarealist.forEach(element => {

          this.selectedarea.push(element.testingAreaID)
        })

      }
      else {
        this.checkedtestarea = false
        this.selectedarea = [];
      }
    }
    else if (type == "Product") {
      if (event.target.checked == true) {
        this.checkedproducttype = true
        this.ProductTypeList.forEach(element => {

          this.selectedproduct.push(element.typeID)
        })

      }
      else {
        this.checkedproducttype = false
        this.selectedproduct = [];
      }
    }
    else if (type == "Instrument") {
      if (event.target.checked == true) {
        this.checkedinstrumentstatus = true;
      }
      else {
        this.checkedinstrumentstatus = false;
      }
    }
    else if (type == "Site") {
      if (event.target.checked == true) {
        this.checkedsitestatus = true;
      }
      else {
        this.checkedsitestatus = false;
      }
    }
    else if (type == "Site Instrument") {
      if (event.target.checked == true) {
        this.checkedsiteinsstatus = true;
      }
      else {
        this.checkedsiteinsstatus = false;
      }
    }
    else if (type == "Test Product Usage Rate") {
      if (event.target.checked == true) {
        this.checkedtestproductusagestatus = true;
      }
      else {
        this.checkedtestproductusagestatus = false;
      }
    }
    else if (type == "Consumables") {
      if (event.target.checked == true) {
        this.checkedconsumblesstatus = true;
      }
      else {
        this.checkedconsumblesstatus = false;
      }
    }
  }
  checkedproduct(event) {
    if (event.target.checked == true) {
      this.checkedproducttype = true
      this.ProductTypeList.forEach(element => {

        this.selectedproduct.push(element.typeID)
      })

    }
    else {
      this.checkedproducttype = false
      this.selectedproduct = [];
    }
  }
  showdiv(args) {
    console.log(args)
    if (args == "b") {
      this.Shoediv = false
      this.radiobuttonval = args;
      this.btnprocess = "Download";
    }
    else {
      this.Shoediv = true
      this.radiobuttonval = args;
      this.btnprocess = "Process";
    }
  }
  getTestingArea() {
    this._APIwithActionService.getDataList('Test', 'GetAllbyadmin').subscribe((data) => {
      this.Testingarealist = data

      this.Testingarealist.forEach(element => {

        this.selectedarea.push(element.testingAreaID)
      })

      this._APIwithActionService.getDataList('Product', 'GetAllbyadmin').subscribe((data) => {

        this.ProductTypeList = data;

        this.ProductTypeList.forEach(element => {
          this.selectedproduct.push(element.typeID)
        })
      }
      )


      //console.log(this.Instrumentlist)
    }
    ), err => {
      console.log(err);
    }

  }
  togglelist() {
    if (this.showclass == true) {
      this.showclass = false
    }
    else {
      this.showclass = true
    }
  }
  changeLstener(args: any, type: string) {
    if (type == "testingArea") {
      if (args.target.checked == true) {
        this.selectedarea.push(args.target.value);
      }
      else {
        let index = this.selectedarea.findIndex(x => x == args.target.value)
        this.selectedarea.splice(index);
        this.checkedteststatus = false;
      }
    }
    else {
      if (args.target.checked == true) {
        this.selectedproduct.push(args.target.value);
      }
      else {
        let index = this.selectedproduct.findIndex(x => x == args.target.value)
        this.selectedproduct.splice(index);
        this.checkedproductstatus = false;

      }
    }
    console.log(args)
  }
  Downloadsample()
  {
    window.location.href = "https://storage.cloud.google.com/forlabaj.appspot.com/ImportBlankTemplate.xls"
  }
  downloadfile() {
  
    let areaids: string = "";
    let productids: string = "";
    let wsregion: XLSX.WorkSheet;
    let wssite: XLSX.WorkSheet;
    let wsins: XLSX.WorkSheet;
    let wstest: XLSX.WorkSheet;
    let wsproduct: XLSX.WorkSheet;
    let wssiteins: XLSX.WorkSheet;
    let wstestproduct: XLSX.WorkSheet;
    let wsconsumbles: XLSX.WorkSheet;
    let flag: boolean = false
    const wb: XLSX.WorkBook = XLSX.utils.book_new();

    if (this.radiobuttonval == "b") {
      window.location.href = "https://storage.cloud.google.com/forlabaj.appspot.com/ImportBlankTemplate.xls"
    }
    else {
      if (this.btnprocess == "Process") {
        if (this.checkedsitestatus == true) {

          this._APIwithActionService.getDataList('Site', 'Getdefaultdata').subscribe((data) => {
            this.regionlist.push({
              RegionName: "Region",
              ShortName: "Short Name",

            })
            data.region.forEach(element => {
              this.regionlist.push({
                RegionName: element.regionName,
                ShortName: element.shortName,

              })
            });

            this.sitelist.push({
              Region: "Region",
              SiteCategory: "Site Category",
              SiteName: "Site Name",
              WorkingDays: "Working Days"
            })

            data.site.forEach(element => {
              this.sitelist.push({
                Region: element.region,
                SiteCategory: element.siteCategory,
                SiteName: element.siteName,
                WorkingDays: element.workingDays,

              })
            });


          }
          )
        }

        if (this.checkedinstrumentstatus == true) {
          this._APIwithActionService.getDataList('Instrument', 'GetdefaultdataIns').subscribe((data) => {
            this.inslist.push({
              TestingArea: "Testing Area",
              InstrumentName: "Instrument Name",
              maxThroughPut: "Max Through Put",
              PerTestControl: "Per Test Control",
              DailyControlTest: "Daily Control Test",
              WeeklyControlTest: "Weekly Control Test",
              MonthlyControlTest: "Monthly Control Test",
              QuarterlycontrolTest: "Quarterly control Test"
            })

            data.forEach(element => {
              this.inslist.push({
                TestingArea: element.testingArea,
                InstrumentName: element.instrumentName,
                maxThroughPut: element.maxThroughPut,
                PerTestControl: element.maxTestBeforeCtrlTest,
                DailyControlTest: element.dailyCtrlTest,
                WeeklyControlTest: element.weeklyCtrlTest,
                MonthlyControlTest: element.monthlyCtrlTest,
                QuarterlycontrolTest: element.quarterlyCtrlTest
              })
            });


          })
        }
        if (this.selectedarea.length > 0) {
          console.log(this.selectedarea);
          for (let index = 0; index < this.selectedarea.length; index++) {
            areaids = areaids + "," + this.selectedarea[index];

          }
          this._APIwithActionService.getDatabyID(areaids, 'test', 'Getdefaulttest').subscribe((data) => {
            this.testlist.push({
              testName: "Test Name",
              AreaName: "Area Name"
            })


            data.forEach(element => {
              this.testlist.push({
                testName: element.testName,
                AreaName: element.testingArea
              })
            });




          })




          flag = true;



        }
        if (this.selectedproduct.length > 0) {
          for (let index = 0; index < this.selectedproduct.length; index++) {
            productids = productids + "," + this.selectedproduct[index];

          }
          this._APIwithActionService.getDatabyID(productids, 'Product', "getdefaultdatapro").subscribe((data) => {

            this.productlist.push({
              ProductName: "Product Name",
              ProductType: "Product Type",
              SerialNo: "Serial No",
              Specification: "Specification",
              BasicUnit: "Basic Unit",
              Minpackpersite: "Min Packs Per Site",
              RapidTestSpecification: "Rapid Test Specification",
              Price: "Price",
              PackingSize: "Packing Size",
              PriceAsDate: "Price As of Date"
            })


            data.forEach(element => {
              this.productlist.push({
                ProductName: element.productName,
                ProductType: element.productType,
                SerialNo: element.catalog,
                Specification: "",
                BasicUnit: element.basicUnit,
                Minpackpersite: element.minpacksize,
                RapidTestSpecification: "",
                Price: element.packcost,
                PackingSize: element.packsize,
                PriceAsDate: element.priceDate
              })
            });


          })
        }
        if (this.checkedsiteinsstatus == true) {
          this._APIwithActionService.getDataList('site', 'Getdefaultsiteinstrument').subscribe((data) => {

            this.siteinstrument.push({
              Region: "Region",
              SiteName: "Site Name",
              TestingArea: "Testing Area",
              InstrumentName: "Instrument Name",
              Quantity: "Quantity",
              Run: "%Run",

            })
            data.forEach(element => {
              this.siteinstrument.push({
                Region: element.region,
                SiteName: element.site,
                TestingArea: element.testingareaName,
                InstrumentName: element.instrumentName,
                Quantity: element.quantity,
                Run: element.testRunPercentage
              })
            });

          })
        }

        if (this.checkedtestproductusagestatus == true) {
          if (this.selectedarea.length > 0) {
            console.log(this.selectedarea);
            for (let index = 0; index < this.selectedarea.length; index++) {
              areaids = areaids + "," + this.selectedarea[index];

            }
          }
          this._APIwithActionService.getDatabyID(areaids, 'test', 'Getdefaulttestproduct').subscribe((data) => {



            this.testproductusage.push({
              TestName: "Test Name",
              Instrument: "Instrument",
              Product: "Product",
              Rate: "Rate",
              IsForControl: "Is For Control"
            })


            data.forEach(element => {
              this.testproductusage.push({
                TestName: element.test,
                Instrument: element.instrumentName,
                Product: element.productName,
                Rate: element.rate,
                IsForControl: element.isForControl == true ? "1" : "0"
              })
            });


          })

        }
        if (this.checkedconsumblesstatus == true) {
          if (this.selectedarea.length > 0) {
            console.log(this.selectedarea);
            for (let index = 0; index < this.selectedarea.length; index++) {
              areaids = areaids + "," + this.selectedarea[index];

            }
          }
          this._APIwithActionService.getDatabyID(areaids, 'test', 'Getdefaulttestconsumble').subscribe((data) => {

            this.consumbleslist.push({
              TestName: "Test Name",
              Instrument: "Instrument",
              Product: "Product",
              Period: "Period",
              NumberOfTest: "Number Of Test",
              Rate: "Rate",
              PerTest: "Per Test",
              PerPeriod: "Per Period",
              PerInstrument: "Per Instrument"

            })
            data.forEach(element => {
            this.consumbleslist.push({
              TestName: element.test,
              Instrument: element.instrumentName,
              Product: element.productName,
              Period: element.period,
              NumberOfTest: element.noOfTest,
              Rate: element.usageRate,
              PerTest: element.perTest== true ? "1" : "0",
              PerPeriod: element.perPeriod== true ? "1" : "0",
              PerInstrument: element.perInstrument== true ? "1" : "0"

            })
          })

          })

        }


        this.btnprocess = "Download";
      }
      else {
        if(this.regionlist.length>0)
        {
        wsregion = XLSX.utils.json_to_sheet(this.regionlist, { skipHeader: true });

        XLSX.utils.book_append_sheet(wb,wsregion,"Region");
        }
        if(this.sitelist.length>0)
        {
        wssite = XLSX.utils.json_to_sheet(this.sitelist, { skipHeader: true });
        XLSX.utils.book_append_sheet(wb,wssite,"Site");
        }
        if(this.siteinstrument.length>0)
        {
          wssiteins = XLSX.utils.json_to_sheet(this.siteinstrument, { skipHeader: true });
        XLSX.utils.book_append_sheet(wb,wssiteins,"Site Instrument");
        }

        if(this.productlist.length>0)
        {
          wsproduct = XLSX.utils.json_to_sheet(this.productlist, { skipHeader: true });
          XLSX.utils.book_append_sheet(wb,wsproduct,"Product");
        }
        if(this.inslist.length>0)
        {
        wsins = XLSX.utils.json_to_sheet(this.inslist, { skipHeader: true });
        XLSX.utils.book_append_sheet(wb,wsins,"Instrument");
        }

        if(this.testlist.length>0)
        {
          wstest = XLSX.utils.json_to_sheet(this.testlist, { skipHeader: true });
        XLSX.utils.book_append_sheet(wb,wstest,"Test");
        }
        if(this.testproductusage.length>0)
        {
        wstestproduct = XLSX.utils.json_to_sheet(this.testproductusage, { skipHeader: true });
        XLSX.utils.book_append_sheet(wb,wstestproduct,"Test Product Usage Rate");
        }
        if(this.consumbleslist.length>0)
        {
        wsconsumbles=XLSX.utils.json_to_sheet(this.consumbleslist,{skipHeader:true})
        XLSX.utils.book_append_sheet(wb,wsconsumbles,"Consumables");
        }
  // const wb: XLSX.WorkBook = { Sheets: { 'Region': wsregion, 'Site': wssite, 'Site Instrument': wssiteins, 'Product': wsproduct, 'Instrument': wsins, 'Test': wstest, 'Test Product Usage Rate': wstestproduct ,'Consumables':wsconsumbles}, SheetNames: ['Region', 'Site', 'Site Instrument', 'Product', 'Instrument', 'Test', 'Test Product Usage Rate','Consumables'] };

        const excelBuffer: any = XLSX.write(wb, { bookType: 'xlsx', type: 'array', cellStyles: true });
        // this.saveExcelFile(excelBuffer, "Instrument List");
        const data1: Blob = new Blob([excelBuffer], { type: fileType });
        FileSaver.saveAs(data1, "Import Template" + fileExtension);
      }
      // 
      //  //     const co: XLSX.CommonOptions = { cellStyles:true};
      //  XLSX.utils.book_append_sheet


    }





  }

  incomingfile(event) {

    const reader = new FileReader();

    this.file = event.target.files[0];
    this.formData.append(this.file.name, this.file);



    const target: DataTransfer = <DataTransfer>(event.target);
    if (target.files.length !== 1) throw new Error('Cannot use multiple files');

    reader.onload = (e: any) => {
      /* read workbook */
      const bstr: string = e.target.result;
      this.wb = XLSX.read(bstr, { type: 'binary' });

      /* grab first sheet */
   this.Sheetarr = this.wb.SheetNames;

      // this.wb.SheetNames.forEach(x=>{
      //   this.Sheetarr.push(x.trim());
      // })
      // const wsname: string = this.wb.SheetNames[0];
      // const ws: XLSX.WorkSheet = this.wb.Sheets["Hist consumption corrected"];

      // /* save data */
      // var data = XLSX.utils.sheet_to_json(ws, { header: 1 });
    };
    reader.readAsBinaryString(target.files[0]);
    // this.Sheetarr =["Region","Site","Product","Test","Instrument","Test Product Usage Rate","Consumables","Site Instrument"]
    this.lgModal4.show();
  }

  download() {
    window.location.href = "http://forlabplus.com/ImportTemplate.xls"
  }
  Upload() {
    let selectedsheetstr: string = "";
    this.selectedsheet.forEach(x => {
      selectedsheetstr = selectedsheetstr + x + ",";
    })
    var token = localStorage.getItem("jwt");
    //       if (this.file.length === 0)
    //       return;
    // console.log(this.Importform)
    //     const formData = new FormData();


    let params1 = new HttpParams();
    params1.append("sheets", selectedsheetstr)
    //"http://localhost:53234/api/Import/Uploadfile"
    //http://forlab.dataman.net.in/webapi/api/Import/Uploadfile   http://forlab.aspwork.co.in
    //http://forlabplus.com/webapi/api/Import/Uploadfile/
    //https://forlab-174007.appspot.com/api/Import/Uploadfile/
    const uploadReq = new HttpRequest('Post', `http://forlab.aspwork.co.in/api/Import/Uploadfile/` + selectedsheetstr, this.formData, {
      headers: new HttpHeaders({ "Authorization": "Bearer " + token, 'userid': localStorage.getItem("userid"), 'countryid': localStorage.getItem("countryid") }),
      reportProgress: true


    });

    //   this.http.post("http://localhost:53234/api/Import/Uploadfile",formData).subscribe((data)=>{
    //     console.log(data["_body"])
    //   })
    //   this._APIwithActionService.postAPI(formData,"Import","Uploadfile").subscribe((data)=>{
    //       console.log(data["_body"])
    //   })
    this.loading = true;
    this.http.request(uploadReq).subscribe(event => {
      if (event.type === HttpEventType.UploadProgress)
        this.progress = 50;
      else if (event.type === HttpEventType.Response) {
        this.progress = 100;
        this.message = event.body["msg"];
        if (this.message != "") {
          this.loading = false;
          this.btntext = "Done";
          this.messagearr = this.message.split('#');
          this.Show = true
        }
      }
    });

    if (this.messagearr.length > 0) {

    }

    // let fileReader = new FileReader();
    //   fileReader.onload = (e) => {
    //       this.arrayBuffer = fileReader.result;
    //       var data = new Uint8Array(this.arrayBuffer);
    //       var arr = new Array();
    //       for(var i = 0; i != data.length; ++i) arr[i] = String.fromCharCode(data[i]);
    //       var bstr = arr.join("");
    //       var workbook = XLSX.read(bstr, {type:"binary"});
    //       console.log(workbook);
    //      // var first_sheet_name = workbook.SheetNames[1];
    //       var worksheet = workbook.Sheets["Site"];
    //       console.log(XLSX.utils.sheet_to_json(worksheet,{raw:true}));
    //   }

  }
  SelectSheet(ischecked: boolean, Item: string) {

    if (ischecked == true) {
      this.selectedsheet.push(Item);
    }
    else {
      // this.Selectedproductid.findIndex(x=>x===id);
      this.selectedsheet.splice(this.selectedsheet.findIndex(x => x === Item), 1);
    }

  }
  Selectall(ischecked: boolean) {
    if (ischecked == true) {
      this.checkstatus = true
      this.selectedsheet = this.Sheetarr;
    }
    else {
      this.checkstatus = false
      // this.Selectedproductid.findIndex(x=>x===id);
      this.selectedsheet = [];
    }

  }
  selectsheetcls() {
    if (this.selectedsheet.length > 0) {
      this.lgModal4.hide();
    }
    else {
      this._GlobalAPIService.FailureMessage("Please Select Atleast one Sheet")
      return;
    }
  }
}