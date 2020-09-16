import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { ModalDirective } from "ngx-bootstrap";
import {NotificationService} from '../../shared/utils/notification.service';

@Component({
  selector: 'app-forecastmethodology',
  templateUrl: './forecastmethodology.component.html',
  styleUrls: ['./forecastmethodology.component.css']
})
export class ForecastmethodologyComponent implements OnInit {
  forecastid:number;
  content:string="";
  exist:number=0;
  isHighlight1:boolean=false;
  isHighlight2:boolean=false;
  isHighlight3:boolean=false;
  seletedtype:string="";
  methodology:string="";
  @ViewChild('lgModal4') public lgModal4: ModalDirective;
  constructor(private notificationService:NotificationService,private _fb: FormBuilder, private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService) {
      if (this._avRoute.snapshot.params["id"]) {  
        this.forecastid = this._avRoute.snapshot.params["id"];  
       
    
    }  

    this._APIwithActionService.getDatabyID(this.forecastid,'Forecsatinfo','GetbyId').subscribe((resp) => {                  
    
     this.methodology= resp["methodology"]

     switch ( this.methodology) {
      case "SERVICE STATSTICS":
        this.isHighlight1 = true
        this.isHighlight2 = false
        this.isHighlight3 = false
      
        break;
      case "CONSUMPTION":
        this.isHighlight1 = false
        this.isHighlight2 = true
        this.isHighlight3 = false
      
        break;
      case "MORBIDITY":
        this.isHighlight1 = false
        this.isHighlight2 = false
        this.isHighlight3 = true
       
        break;

    }
    
  })
    this._APIwithActionService.getDatabyID(this.forecastid,'Forecsatinfo','Isdataimported').subscribe((resp) => {                  
    this.exist=resp
   console.log(resp)
 
     })
     }

  ngOnInit() {
  }
  ReadMore(type: string)
  {
    if (type == "S") {
      this.content = "Serng servevice statistics data are also historical, program-level or facility-level data on the number of patient visits to facilities, the number of services provided, or the number of people who received a specific service or treatment within a given time period. Service statistics data can be found in program monitoring reports, HMIS data, facility-level data on service utilization and attendance rates, or in patient records. In some programs, the LMIS captures a limited number of service statistics. For ARVs, service statistics data would be the total number of ART patients on treatment at a facility, or perhaps the total number of patient visits to a facility at a point in time. For HIV tests, service statistics would be the total number of clients tested during a certain period. For laboratory supplies, service statistics are the total number of tests performed in a certain period (e.g., CD4 count testsperformed in a given quarter).";
    }
    else if (type == "C") {
      this.content = "Consumption data are historical data on the actual quantities of a product that have been dispensed to patients or used at a service delivery point within a given time period, and are typically reported per month or per quarter. Daily consumption data can be found in pharmacy dispensing registers, laboratory registers, or other point of service registers. Where a well-functioning LMIS captures and aggregates these data from service delivery points, aggregated consumption data can be found in monthly and annual facility-level and program-level reports. For antiretroviral drugs (ARVs), consumption data would be the actual quantity of each ARV dispensed to ART patients. For HIV tests, consumption data or “usage data” are the actual number of HIV tests used over a given period. For laboratory supplies, consumption data are the actual number of laboratory commodities used.";
    }
    else {
      this.content = "MORBIDITY";
    }
    this.lgModal4.show();
  }
  updateforecast(type: string) {
    let methodology: string = "";
    this.seletedtype=type;
    if (type == "S") {
      methodology = "SERVICE STATSTICS";
    }
    else if (type == "C") {
      methodology = "CONSUMPTION";
    }
    else {
      methodology = "MORBIDITY";
    }
    this._APIwithActionService.putAPI( this.forecastid,methodology,"Forecsatinfo","Put").subscribe((data)=>{
      if (type == "D") {
        this._router.navigate(["/Forecast/DemographicProgramlist", this.forecastid]);
      }
      else
      {
      this._router.navigate(["/Forecast/Forecasttype", this.forecastid]);
      }
      
    })
    console.log(methodology);
  }
  Previousclick()
  {
    this.notificationService.smallBox({
      title: "Conformation!",
      content: "Would like to Cancel for this page? <p class='text-align-right'><a href='/#/Forecast/ForecastProductusage/" + this.forecastid + "' class='btn btn-primary btn-sm'>Yes</a> <a href-void class='btn btn-danger btn-sm'>No</a></p>",
      color: "#296191",
      //timeout: 8000,
      icon: "fa fa-bell swing animated"
    });
    // this._router.navigate(["/Forecast/ForecastProductusage", this.forecastid]);
  }
  Saveforecastproductusage()
  {

 if(this.methodology=="MORBIDITY")
 {
  this._router.navigate(["/Forecast/DemographicProgramlist", this.forecastid]);
}
else
{
  this._router.navigate(["/Forecast/Forecasttype", this.forecastid]);
}
 
  
  
  }
}
