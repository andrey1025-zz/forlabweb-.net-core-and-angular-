import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { NgForm } from '@angular/forms';
import { ModalDirective } from "ngx-bootstrap";
import { ngxLoadingAnimationTypes, NgxLoadingComponent } from 'ngx-loading';
import {NotificationService} from '../../shared/utils/notification.service';
import * as FileSaver from 'file-saver';
const PrimaryWhite = '#ffffff';
const SecondaryGrey = '#ccc';
const PrimaryRed = '#dd0031';
const SecondaryBlue = '#006ddd';
const fileType = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
const fileExtension = '.xlsx';
@Component({
  selector: 'app-forecastcalculationmethod',
  templateUrl: './forecastcalculationmethod.component.html',
  styleUrls: ['./forecastcalculationmethod.component.css']
})
export class ForecastcalculationmethodComponent implements OnInit {
  forecastid: number = 0;
  parameterForm: NgForm;
  WastageRate: number = 0;
  Months: number = 3;
  Scaleup: number = 0;
  Methodology: string = "";
  Forecastcalculationmethod: string = "Simplemovingaverage";
  content: string;
  isHighlight1: boolean = false;
  isHighlight2: boolean = false;
  isHighlight3: boolean = false;
  isHighlight4: boolean = false;
  @ViewChild('lgModal4') public lgModal4: ModalDirective;
  @ViewChild('lgModal3') public lgModal3: ModalDirective;
  @ViewChild('ngxLoading') ngxLoadingComponent: NgxLoadingComponent;
  @ViewChild('customLoadingTemplate') customLoadingTemplate: TemplateRef<any>;
  public primaryColour = PrimaryRed;
  public secondaryColour = SecondaryBlue;
  public coloursEnabled = false;
  public loadingTemplate: TemplateRef<any>;
  public ngxLoadingAnimationTypes = ngxLoadingAnimationTypes;
  public config = { animationType: ngxLoadingAnimationTypes.none, primaryColour: this.primaryColour, secondaryColour: this.secondaryColour, tertiaryColour: this.primaryColour, backdropBorderRadius: '3px' };
  public loading = false;
  constructor(private notificationService:NotificationService,private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService) {

    if (this._avRoute.snapshot.params["id"]) {
      this.forecastid = this._avRoute.snapshot.params["id"];


    }
    if (this.forecastid > 0) {
      this._APIwithActionService.getDatabyID(this.forecastid, 'Forecsatinfo', 'GetbyId').subscribe((resp) => {

        this.Methodology = resp["methodology"];
        this.WastageRate = resp["westage"];
        this.Scaleup = resp["scaleup"];
        this.Forecastcalculationmethod = resp["method"];
        switch (resp["method"]) {
          case "Specifiedpercentagegrowth":
            this.isHighlight1 = true
            this.isHighlight2 = false
            this.isHighlight3 = false
            this.isHighlight4 = false
           
            break;
          case "Simplemovingaverage":
            this.isHighlight1 = false
            this.isHighlight2 = true
            this.isHighlight3 = false
            this.isHighlight4 = false
            break;
          case "Weightedmovingaverage":
            this.isHighlight1 = false
            this.isHighlight2 = false
            this.isHighlight3 = true
            this.isHighlight4 = false
            break;
          case "Simplelinearregression":
            this.isHighlight1 = false
            this.isHighlight2 = false
            this.isHighlight3 = false
            this.isHighlight4 = true
            break;
          default:
            break;
        }

      })
    }
  }

  ngOnInit() {
  }
  Previousclick() {

    this.notificationService.smallBox({
      title: "Conformation!",
      content: "Would like to Cancel for this page? <p class='text-align-right'><a href='/#/Forecast/forecastFactor/" + this.forecastid + "' class='btn btn-primary btn-sm'>Yes</a> <a href-void class='btn btn-danger btn-sm'>No</a></p>",
      color: "#296191",
      //timeout: 8000,
      icon: "fa fa-bell swing animated"
    });
    //this._router.navigate(["/Forecast/forecastFactor", this.forecastid]);
  }
  forecastmethod(calculationmethod: string) {
    this.Forecastcalculationmethod = calculationmethod
    if (calculationmethod == "Specifiedpercentagegrowth") {

    }
    else if (calculationmethod == "Weightedmovingaverage" || calculationmethod == "Simplemovingaverage") {
      this.lgModal3.show();
    }
    else {

    }
    switch (calculationmethod) {
      case "Specifiedpercentagegrowth":
        this.isHighlight1 = true
        this.isHighlight2 = false
        this.isHighlight3 = false
        this.isHighlight4 = false
        break;
      case "Simplemovingaverage":
        this.isHighlight1 = false
        this.isHighlight2 = true
        this.isHighlight3 = false
        this.isHighlight4 = false
        break;
      case "Weightedmovingaverage":
        this.isHighlight1 = false
        this.isHighlight2 = false
        this.isHighlight3 = true
        this.isHighlight4 = false
        break;
      case "Simplelinearregression":
        this.isHighlight1 = false
        this.isHighlight2 = false
        this.isHighlight3 = false
        this.isHighlight4 = true
        break;
      default:
        break;
    }

  }
  selectedmonth(form: NgForm) {
    this.Months = form.value.Months
    this.lgModal3.hide();
  }
  Runforecast() {
    this.loading = true;
    this._APIwithActionService.getDatabyID(this.forecastid, 'Conductforecast', 'Calculateforecast', "MethodType=" + this.Forecastcalculationmethod + "," + this.WastageRate + "," + this.Scaleup + "," + this.Months).subscribe((data) => {
      this.loading = false;
      if (this.Methodology == 'CONSUMPTION')
        this._router.navigate(["/Forecast/ForecastChartnew", this.forecastid]);

      else if (this.Methodology == 'SERVICE STATSTICS')
      
      this._router.navigate(["/Forecast/ForecastChartservice", this.forecastid]);

    })
  }
  ReadMore(type: string) {
    if (type == "S") {
      this.content = "SERVICE STATSTICS";
    }
    else if (type == "C") {
      this.content = "CONSUMPTION";
    }
    else {
      this.content = "MORBIDITY";
    }
    this.lgModal4.show();
  }
}
