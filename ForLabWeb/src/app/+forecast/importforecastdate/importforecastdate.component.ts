import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { ModalDirective } from "ngx-bootstrap";
import * as XLSX from 'xlsx';
import { ngxLoadingAnimationTypes, NgxLoadingComponent } from 'ngx-loading';
import { NgForm } from '@angular/forms';
import {NotificationService} from '../../shared/utils/notification.service';
import { HttpClient, HttpRequest, HttpEventType, HttpResponse, HttpHeaders } from '@angular/common/http'
const PrimaryWhite = '#ffffff';
const SecondaryGrey = '#ccc';
const PrimaryRed = '#dd0031';
const SecondaryBlue = '#006ddd';
@Component({
  selector: 'app-importforecastdate',
  templateUrl: './importforecastdate.component.html',
  styleUrls: ['./importforecastdate.component.css']
})
export class ImportforecastdateComponent implements OnInit {
  wb: XLSX.WorkBook;
  Sheetarr = new Array();
  file: File;
  importForm: NgForm
  forecastid: number;
  Filenamenew: string = "Include some File";
  importedlist = new Array();
  stringarr: string = "S";
  loading: boolean = false;
  methodology: string = "";
  formData = new FormData();
  title:string="Import your patient data";
  @ViewChild('lgModal4') public lgModal4: ModalDirective;
  @ViewChild('ngxLoading') ngxLoadingComponent: NgxLoadingComponent;
  @ViewChild('customLoadingTemplate') customLoadingTemplate: TemplateRef<any>;
  public ngxLoadingAnimationTypes = ngxLoadingAnimationTypes;

  public primaryColour = PrimaryRed;
  public secondaryColour = SecondaryBlue;
  public coloursEnabled = false;
  public loadingTemplate: TemplateRef<any>;

  public config = { animationType: ngxLoadingAnimationTypes.none, primaryColour: this.primaryColour, secondaryColour: this.secondaryColour, tertiaryColour: this.primaryColour, backdropBorderRadius: '3px' };

  constructor(private notificationService:NotificationService,private http: HttpClient, private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService) {


    if (this._avRoute.snapshot.params["id"]) {
      this.forecastid = this._avRoute.snapshot.params["id"];

      this._APIwithActionService.getDatabyID(this.forecastid, 'Forecsatinfo', 'GetbyId').subscribe((resp) => {
        console.log(resp)
        this.stringarr = resp["forecastType"];
        this.methodology = resp["methodology"];
        switch (this.methodology) {
          case 'CONSUMPTION':
            this._APIwithActionService.getDatabyID(this.forecastid, 'Import', 'getimporteddata').subscribe((resp) => {
              this.importedlist = resp;

            })
            this.title="IMPORT YOUR HISTORICAL SERVICE OR CONSUMPTION DATA HERE"
            break;
          case 'SERVICE STATSTICS':
            this._APIwithActionService.getDatabyID(this.forecastid, 'Import', 'getimportedservicedata').subscribe((resp) => {
              this.importedlist = resp;

            })
            this.title="IMPORT YOUR HISTORICAL SERVICE OR CONSUMPTION DATA HERE"
            break;
          // default:
          //   break;
        }

      })






    }
  }
Previousclick()
{
  this.notificationService.smallBox({
    title: "Conformation!",
    content: "Would like to Cancel for this page? <p class='text-align-right'><a href='/#/Forecast/Forecasttype/" + this.forecastid + "' class='btn btn-primary btn-sm'>Yes</a> <a href-void class='btn btn-danger btn-sm'>No</a></p>",
    color: "#296191",
    //timeout: 8000,
    icon: "fa fa-bell swing animated"
  });
}
  ngOnInit() {
  }
  incomingfile(event) {

    if (this.methodology == "MORBIDITY") {
      this.file = event.target.files[0];
      this.Filenamenew = this.file.name;
      this.formData.append(this.file.name, this.file);
      var token = localStorage.getItem("jwt");
      const uploadReq = new HttpRequest('PUT', `https://forlab-174007.appspot.com/api/Import/Importpatient/` + this.forecastid, this.formData, {
        headers: new HttpHeaders({ "Authorization": "Bearer " + token, 'userid': localStorage.getItem("userid"), 'countryid': localStorage.getItem("countryid") }),
        reportProgress: true,
      });

      this.http.request(uploadReq).subscribe(event => {
        if (this.stringarr == 'S') {
          this._router.navigate(["/Forecast/Demographicsitebysite", this.forecastid]);
        }
        else {
          this._router.navigate(["/Forecast/DemographicAggregrate", this.forecastid]);
        }
        // this._APIwithActionService.Getpatientnumber.emit(event["body"])

        // this.lgModal3.hide();
      })
    }
    else {
      this.file = event.target.files[0];
      this.Filenamenew = this.file.name;
      console.log(this.file);
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
        let ws: XLSX.WorkSheet;

        switch (this.methodology) {

          case "CONSUMPTION":
            ws = this.wb.Sheets["Historical Consumption"];
            var data1 = XLSX.utils.sheet_to_json(ws, { header: 1 });
            console.log(data1[3][0])

            let arr = new Array()


            for (let index = 1; index < data1.length; index++) {

              arr.push(data1[index])

            }

            for (let index = 4; index < arr[0].length; index++) {
              let date1 = new Date(arr[0][index])
              arr[0][index] = date1.getDate().toString() + "/" + (date1.getMonth() + 1) + "/" + date1.getFullYear();
              console.log(date1)
            }

            let date1 = new Date(data1[1][4])
            console.log(date1)
            console.log(date1.getMonth() + 1);
            console.log(date1.getDate());
            console.log(date1.getFullYear());

            console.log(arr);
            //  this.loading=true;
            //  this.lgModal6.show();
            this.loading = true;

            this._APIwithActionService.putAPI(this.forecastid, arr, 'Import', 'importconsumption').subscribe((data: any) => {


              this.importedlist = JSON.parse(data["_body"]);

              this.loading = false;

              //   console.log(this.importedlist);




            })
            break;
          case "SERVICE STATSTICS":
            ws = this.wb.Sheets["Historical Service Data"];
            var data1 = XLSX.utils.sheet_to_json(ws, { header: 1 });
            console.log(data1[3][0])

            let arr1 = new Array()


            for (let index = 1; index < data1.length; index++) {

              arr1.push(data1[index])

            }

            for (let index = 4; index < arr1[0].length; index++) {
              let date1 = new Date(arr1[0][index])
              // console.log(date1)
              arr1[0][index] = date1.getDate().toString() + "/" + (date1.getMonth() + 1) + "/" + date1.getFullYear();

              console.log(arr1[0][index])
            }
            //  this.loading=true;
            //  this.lgModal6.show();
            this.loading = true;

            this._APIwithActionService.putAPI(this.forecastid, arr1, 'Import', 'importservice').subscribe((data: any) => {


              this.importedlist = JSON.parse(data["_body"]);

              this.loading = false;

              console.log(this.importedlist);




            })
            break;
        }

        /* save data */
        var data = XLSX.utils.sheet_to_json(ws, { header: 1 });
      };
      reader.readAsBinaryString(target.files[0]);
    }
    //  this.lgModal4.show();
  }
  SelectSheet(sheetname) {


    this.lgModal4.hide();
    const ws: XLSX.WorkSheet = this.wb.Sheets[sheetname];

    /* save data */
    var data1 = XLSX.utils.sheet_to_json(ws, { header: 1 });
    //  console.log(data1[3][0])

    let arr = new Array()


    for (let index = 1; index < data1.length; index++) {

      arr.push(data1[index])

    }
    console.log(arr);
    //  this.loading=true;
    //  this.lgModal6.show();
    this._APIwithActionService.putAPI(this.forecastid, arr, 'Import', 'importconsumption').subscribe((data: any) => {


      this.importedlist = JSON.parse(data["_body"]);








    })

  }
  Saveforecastimportdata() {
    let Reportobject: Object;
    Reportobject = {
      receivereportdata: this.importedlist
    }
    console.log(Reportobject["receivereportdata"])

    if (this.methodology == "SERVICE STATSTICS") {
      if (Reportobject["receivereportdata"].length > 0) {
        this._APIwithActionService.postAPI(Reportobject, 'Import', 'saveimportservice').subscribe((data: any) => {

          this.lgModal4.hide();
          this._GlobalAPIService.SuccessMessage(data["_body"]);
          this._router.navigate(["/Forecast/forecastFactor", this.forecastid]);


        })
      }

      else {
        this._router.navigate(["/Forecast/forecastFactor", this.forecastid]);
      }
    }
    else if (this.methodology == "CONSUMPTION") {
      if (Reportobject["receivereportdata"].length > 0) {
        this._APIwithActionService.postAPI(this.importedlist, 'Import', 'saveimportconsumption').subscribe((data: any) => {

          this.lgModal4.hide();
          this._GlobalAPIService.SuccessMessage(data["_body"]);
          this._router.navigate(["/Forecast/forecastFactor", this.forecastid]);


        })
      }

      else {

      


        this._router.navigate(["/Forecast/forecastFactor", this.forecastid]);
      }


    

    }
    else
    {
      if (this.stringarr == 'S') {
        this._router.navigate(["/Forecast/Demographicsitebysite", this.forecastid]);
      }
      else {
        this._router.navigate(["/Forecast/DemographicAggregrate", this.forecastid]);
      }
    }

  }
}
