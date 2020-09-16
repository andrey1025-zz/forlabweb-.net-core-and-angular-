import { Component, OnInit } from '@angular/core';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { Router } from '@angular/router';  
@Component({
  selector: 'app-filterforecastsummary',
  templateUrl: './filterforecastsummary.component.html'
})
export class FilterforecastsummaryComponent implements OnInit {
  ForecastList = new Array();
  forecastid:any;
  Methodtype:any;
  constructor(private _APIwithActionService: APIwithActionService,private _router:Router) { }

  ngOnInit() {
  }
  getforecastbymethod(value: any) {
    if (value != "DEMOGRAPHIC")
      this._APIwithActionService.getDatabyID(value, "MMProgram", "GetForecastInfoByMethodology","CID="+localStorage.getItem("countryid")).subscribe((data) => {
        this.ForecastList = data
      })
    else {
      this._APIwithActionService.getDatabyID("MORBIDITY", "MMProgram", "GetForecastInfoByMethodology","CID="+localStorage.getItem("countryid")).subscribe((data) => {
        this.ForecastList = data
      })
    }
  }

  openreport()
{
 
  this._router.navigate(["Report/viewforecastsummary",this.forecastid,this.Methodtype])
}
}
